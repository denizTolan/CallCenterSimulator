﻿using System;
using System.Collections.Generic;
using System.Text;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Agent.Domain.Commands
{
    public abstract class TransferCommand : Command
    {
        public Guid TransactionId { get; protected set; }
        public string UserName { get; protected set; }
        public DateTime CreatedTime { get; protected set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
    }
}
