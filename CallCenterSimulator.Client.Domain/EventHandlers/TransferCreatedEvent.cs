using System;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Client.Domain.EventHandlers
{
    public class TransferCreatedEvent : Event
    {
        public Guid TransactionId { get; private set; }
        public string UserName { get; private set; }
        public DateTime CreatedTime { get; private set; }

        public TransferCreatedEvent(Guid transactionId,string userName,DateTime createdTime)
        {
            this.TransactionId = transactionId;
            this.UserName = userName;
            this.CreatedTime = createdTime;
        }
    }
}
