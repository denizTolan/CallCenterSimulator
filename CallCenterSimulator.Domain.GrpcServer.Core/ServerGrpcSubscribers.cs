
using CallCenterSimulator.Domain.GrpcServer.Core.Abstract;
using CallCenterSimulator.Domain.GrpcServer.Core.Model;
using Microsoft.Extensions.Logging;
using NotificationGrpcService;

namespace CallCenterSimulator.Domain.GrpcServer.Core
{
    public class ServerGrpcSubscribers : ServerGrpcSubscribersBase<RegisterResponse>
    {
        public ServerGrpcSubscribers(ILoggerFactory loggerFactory) 
            : base(loggerFactory)
        {
        }

        public override bool Filter(SubscribersModel<RegisterResponse> subscriber, RegisterResponse message) => true;
    }
}