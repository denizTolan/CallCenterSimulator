using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CallCenterSimulator.Client.Domain.Commands;
using CallCenterSimulator.Client.Domain.EventHandlers;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Client.Domain.CommandHandlers
{
    public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _bus;

        public TransferCommandHandler(IEventBus bus)
        {
            this._bus = bus;
        }

        public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            this._bus.Publish<TransferCreatedEvent>(new TransferCreatedEvent(request.TransactionId,request.UserName,request.CreatedTime));

            return Task.FromResult(true);
        }
    }
}
