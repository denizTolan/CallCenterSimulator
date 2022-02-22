using System;
using System.Collections.Generic;
using System.Text;

namespace CallCenterSimulator.Agent.Domain.Commands
{
    public class CreateAgentCommand : TransferCommand
    {
        
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
