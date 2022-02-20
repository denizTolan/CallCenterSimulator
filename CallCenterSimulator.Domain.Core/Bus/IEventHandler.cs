using System.Threading.Tasks;

namespace CallCenterSimulator.Domain.Core
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    { 
        
    }
}
