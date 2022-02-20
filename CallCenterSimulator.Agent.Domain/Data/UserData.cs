using System;
using System.Collections.Concurrent;
using CallCenterSimulator.Agent.Domain.Events;
using CallCenterSimulator.Agent.Domain.Interface;

namespace CallCenterSimulator.Agent.Domain.Data
{
    public class UserData : IUserData
    {
        private readonly ConcurrentQueue<TransferCreatedEvent> _concurrentQueue;

        public UserData()
        {
            this._concurrentQueue = new ConcurrentQueue<TransferCreatedEvent>();
        }
        
        public void AddUserQueue(TransferCreatedEvent transferCreatedEvent)
        {
            this._concurrentQueue.Enqueue(transferCreatedEvent);
        }

        public TransferCreatedEvent PopUserData()
        {
            TransferCreatedEvent firstData;
            if (!this._concurrentQueue.TryDequeue(out firstData))
                throw new Exception("Queue dont have an any data.");
            
            return firstData;
        }
    }
}