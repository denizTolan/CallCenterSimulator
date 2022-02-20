
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CallCenterSimulator.Agent.Domain.Events;
using CallCenterSimulator.Agent.Domain.Interface;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Agent.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        private readonly IUserData _userData;
        
        public TransferEventHandler(IUserData userData)
        {
            this._userData = userData;
        }

        public Task Handle(TransferCreatedEvent @event)
        {
            this._userData.AddUserQueue(@event);
            return Task.CompletedTask;
        }
    }
}
