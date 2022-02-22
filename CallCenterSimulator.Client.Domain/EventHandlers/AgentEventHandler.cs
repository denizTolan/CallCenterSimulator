using System.Threading.Tasks;
using CallCenterSimulator.Client.Domain.Events;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Client.Domain.EventHandlers
{
    public class AgentEventHandler : IEventHandler<AgentCreatedEvent>
    {
        
        public AgentEventHandler()
        {
            
        }

        public Task Handle(AgentCreatedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}