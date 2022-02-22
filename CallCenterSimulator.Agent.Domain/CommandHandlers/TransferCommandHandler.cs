using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CallCenterSimulator.Agent.Domain.Commands;
using CallCenterSimulator.Agent.Domain.Events;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Agent.Domain.CommandHandlers
{
    public class TransferCommandHandler : IRequestHandler<CreateAgentCommand, bool>
    {
        private readonly IEventBus _bus;

        public TransferCommandHandler(IEventBus bus)
        {
            this._bus = bus;
        }

        public Task<bool> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
        {
            this._bus.Publish<TransferCreatedEvent>(new TransferCreatedEvent(request.TransactionId,request.UserName,request.CreatedTime));

            return Task.FromResult(true);
        }
    }
}
