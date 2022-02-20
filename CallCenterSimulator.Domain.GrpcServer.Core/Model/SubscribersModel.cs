using Grpc.Core;

namespace CallCenterSimulator.Domain.GrpcServer.Core.Model
{
    public class SubscribersModel<TResponse>
    {
        public IServerStreamWriter<TResponse> Subscriber { get; set; }

        public string Id { get; set; }
    }
}