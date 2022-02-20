using System;
using System.Threading;
using System.Threading.Tasks;
using CallCenterSimulator.Domain.GrpcClient.Core.Model.Events;
using Grpc.Core;
using Channel = Grpc.Core.Channel;

namespace CallCenterSimulator.Domain.GrpcClient.Core
{
    public abstract class GrpcClientBase<TRequest, TResponse> : IDisposable
    {
        protected readonly Channel _channel;
        protected readonly AsyncDuplexStreamingCall<TRequest, TResponse> _duplexClient;
        
        public event EventHandler<ProcessEventArgs> OnCallStarted;
        public event EventHandler<ProcessEventArgs> OnShuttingDown;
        public event EventHandler OnServerCreatedMessage;
            
        public GrpcClientBase(Channel channel)
        {
            this._channel = channel;
            this._duplexClient = CreateDuplexClient(channel);
        }
        
        public abstract AsyncDuplexStreamingCall<TRequest, TResponse> CreateDuplexClient(Channel channel);

        public abstract TRequest CreateMessage(object ob);

        public abstract string MessagePayload { get; }

        public async Task StartCall(string userName)
        {
            if (OnCallStarted != null)
            {
                OnCallStarted.Invoke("OnCallStarted",new ProcessEventArgs()
                {
                    CompletionTime = DateTime.Now,
                    IsSuccessful = true
                });
            }

            //Register
            await this._duplexClient.RequestStream.WriteAsync(CreateMessage(userName));
        }
        
        public async Task<Status> GetConnectionStatus()
        {
            await this._channel.WaitForStateChangedAsync(ChannelState.Ready,null);
            return await Task.FromResult<Status>(this._duplexClient.GetStatus());
        }

        public virtual Task ListenServer()
        {
            var responseTask = Task.Run(async () =>
            {
                while (await this._duplexClient.ResponseStream.MoveNext(CancellationToken.None))
                {
                    
                    Console.WriteLine($"{this._duplexClient.ResponseStream.Current}");
                }
            });

            return responseTask;
        }

        public virtual async Task ListenServerAsync()
        {
            while (await this._duplexClient.ResponseStream.MoveNext(CancellationToken.None))
            {
                if (this.OnServerCreatedMessage != null)
                    this.OnServerCreatedMessage.Invoke(this._duplexClient.ResponseStream.Current,null);
                    
                Console.WriteLine($"{this._duplexClient.ResponseStream.Current}");
            }
        }

        public void ShutDownClient()
        {
            this._duplexClient.RequestStream.CompleteAsync();
            this._channel.ShutdownAsync();
            
            if (this.OnShuttingDown != null)
            {
                this.OnShuttingDown.Invoke("OnShuttingDown",new ProcessEventArgs()
                {
                    CompletionTime = DateTime.Now,
                    IsSuccessful = true
                });
            }
        }

        public void Dispose()
        {
            this._duplexClient.Dispose();
        }
    }
}