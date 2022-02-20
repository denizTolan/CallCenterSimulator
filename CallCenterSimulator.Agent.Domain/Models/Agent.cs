using System.Collections.Generic;
using CallCenterSimulator.Agent.Domain.Common;

namespace CallCenterSimulator.Agent.Domain.Models
{
    public class Agent : EntityBase
    {
        public string AgentName { get; set; }
        public AgentTitleName AgentTitle { get; set; }

        public virtual Team Team { get; set; }
    }
}