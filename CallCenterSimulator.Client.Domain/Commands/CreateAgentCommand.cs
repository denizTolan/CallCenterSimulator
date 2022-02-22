using System;
using System.Collections.Generic;
using System.Text;

namespace CallCenterSimulator.Client.Domain.Commands
{
    public class CreateAgentCommand : TransferCommand
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        
        public CreateAgentCommand(Guid transactionId,string userName,DateTime createdTime,int agentId,string agentName)
        {
            this.TransactionId = transactionId;
            this.UserName = userName;
            this.CreatedTime = createdTime;
            this.AgentId = agentId;
            this.AgentName = agentName;
        }
    }
}
