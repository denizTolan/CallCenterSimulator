using System.Threading.Tasks;
using CallCenterSimulator.Agent.Domain.Events;
using CallCenterSimulator.Agent.Domain.Interface;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Agent.Domain.EventHandlers
{
    public class AgentEventHandler : IEventHandler<AgentCreatedEvent>
    {
        private readonly IUserData _userData;
        
        public AgentEventHandler(IUserData userData)
        {
            this._userData = userData;
        }

        public Task Handle(AgentCreatedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}