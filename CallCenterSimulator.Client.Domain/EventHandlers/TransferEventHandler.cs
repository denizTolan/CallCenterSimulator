using System.Threading.Tasks;
using CallCenterSimulator.Client.Domain.EventHandlers;
using CallCenterSimulator.Domain.Core;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {

        public TransferEventHandler()
        {
        }

        public Task Handle(TransferCreatedEvent @event)
        {

            return Task.CompletedTask;
        }
    }
}
