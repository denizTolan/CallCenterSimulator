using System.Collections.Generic;
using CallCenterSimulator.Agent.Domain.Events;

namespace CallCenterSimulator.Agent.Domain.Interface
{
    public interface IUserData
    {
        void AddUserQueue(TransferCreatedEvent transferCreatedEvent);

        TransferCreatedEvent PopUserData();

        IEnumerable<TransferCreatedEvent> PopUserDataList();
    }
}