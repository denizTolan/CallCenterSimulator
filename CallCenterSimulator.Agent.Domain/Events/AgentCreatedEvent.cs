using System;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Agent.Domain.Events
{
    public class AgentCreatedEvent : Event
    {
        public Guid TransactionId { get; protected set; }
        public string UserName { get; protected set; }
        public DateTime CreatedTime { get; protected set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        
        public AgentCreatedEvent(Guid transactionId,string userName,DateTime createdTime,int agentId,string agentName)
        {
            this.TransactionId = transactionId;
            this.UserName = userName;
            this.CreatedTime = createdTime;
            this.AgentId = agentId;
            this.AgentName = agentName;
        }
    }
}