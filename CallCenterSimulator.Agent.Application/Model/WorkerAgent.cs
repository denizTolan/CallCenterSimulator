using System;
using CallCenterSimulator.Agent.Domain.Data;

namespace CallCenterSimulator.Agent.Application.Model
{
    public class WorkerAgent
    {
        public Domain.Models.Agent Agent { get; set; }
        public string UserName { get; set; }
        public Guid TransactionId { get; set; }
    }
}