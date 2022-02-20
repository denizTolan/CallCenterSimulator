using System;
using System.Threading.Tasks;
using CallCenterSimulator.Client.Api.Abstract;
using CallCenterSimulator.Client.Domain.Commands;
using CallCenterSimulator.Domain.Core;
using CallCenterSimulator.Domain.GrpcServer.Core;
using CallCenterSimulator.Domain.GrpcServer.Core.Model;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NotificationGrpcService;

namespace CallCenterSimulator.Client.Api.Services
{
    public class NotificationService : Notification.NotificationBase , IDisposable
    {
        private readonly GrpcService<RegisterRequest, RegisterResponse> _grpcService;
        private readonly IEventBus _bus;

        public NotificationService(ILoggerFactory loggerFactory, ServerGrpcSubscribers serverGrpcSubscribers, MessageProcessor messageProcessor, IEventBus bus)
        {
            this._grpcService = new GrpcService<RegisterRequest, RegisterResponse>(loggerFactory,serverGrpcSubscribers,messageProcessor);
            _bus = bus;
        }

        public override async Task Register(IAsyncStreamReader<RegisterRequest> requestStream, 
            IServerStreamWriter<RegisterResponse> responseStream, ServerCallContext context)
        {  
            var subscriber = new SubscribersModel<RegisterResponse>
            {
                Subscriber = responseStream
            };
            
            var createTransferCommand = new CreateTransferCommand(
                Guid.NewGuid(), 
                "test",
                DateTime.Now
            );

            _bus.SendCommand(createTransferCommand);
            
             await  this._grpcService.CreateDuplexStreaming(requestStream,responseStream,context);
        }

        public void Dispose()
        {
            this._grpcService.Dispose();
        }
    }
}