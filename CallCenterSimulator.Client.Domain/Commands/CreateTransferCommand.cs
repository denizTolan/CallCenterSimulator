using System;
using System.Collections.Generic;
using System.Text;

namespace CallCenterSimulator.Client.Domain.Commands
{
    public class CreateTransferCommand : TransferCommand
    {
        public CreateTransferCommand(Guid transactionId,string userName,DateTime createdTime)
        {
            this.TransactionId = transactionId;
            this.UserName = userName;
            this.CreatedTime = createdTime;  
        }
    }
}
