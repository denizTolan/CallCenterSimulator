using System.Threading.Tasks;
using CallCenterSimulator.Client.Domain.Commands;
using CallCenterSimulator.Domain.GrpcServer.Core.Abstract;
using CallCenterSimulator.Domain.GrpcServer.Core.Model;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CallCenterSimulator.Client.Api.Abstract
{
    public sealed class GrpcService<TRequest,TResponse>
        where TRequest : class
    {
        private readonly ServerGrpcSubscribersBase<TResponse> _serverGrpcSubscribersBase;
        private readonly MessageProcessorBase<TRequest, TResponse> _messageProcessor;

        private ILogger Logger { get; set; }

        public GrpcService(
            ILoggerFactory loggerFactory, 
            ServerGrpcSubscribersBase<TResponse> serverGrpcSubscribersBase, 
            MessageProcessorBase<TRequest, TResponse> messageProcessor)
        {
            _serverGrpcSubscribersBase = serverGrpcSubscribersBase;
            _messageProcessor = messageProcessor;
            Logger = loggerFactory.CreateLogger<GrpcService<TRequest, TResponse>>();
        }

        public async Task CreateDuplexStreaming(
            IAsyncStreamReader<TRequest> requestStream, 
            IServerStreamWriter<TResponse> responseStream, 
            ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            Logger.LogInformation($"Connection id: {httpContext.Connection.Id}");

            if (!await requestStream.MoveNext())
                return;

            var clientId = _messageProcessor.GetClientId(requestStream.Current);
            Logger.LogInformation($"{clientId} connected");
            var subscriber = new SubscribersModel<TResponse>
            {
                Subscriber = responseStream,
                Id = $"{clientId}"
            };

            _serverGrpcSubscribersBase.AddSubscriber(subscriber);
            
          

            do
            {
                if (requestStream.Current == null)
                    continue;

                var resultMessage = _messageProcessor.Process(requestStream.Current);
                if (resultMessage == null)
                    continue;

                await _serverGrpcSubscribersBase.BroadcastMessageAsync(resultMessage);
            } while (await requestStream.MoveNext());

            _serverGrpcSubscribersBase.RemoveSubscriber(subscriber);
            Logger.LogInformation($"{clientId} disconnected");
        }

        public void Dispose()
        {
            Logger.LogInformation("Cleaning up");
        }
    }
}