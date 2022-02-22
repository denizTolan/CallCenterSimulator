using System.Collections.Generic;
using System.Linq;
using CallCenterSimulator.Agent.Application.Model;

namespace CallCenterSimulator.Agent.Application.Extenstions
{
    public static class AgentExtension
    {
        public static List<Domain.Models.Agent> ToAgentList(this List<WorkerAgent> workerAgentList)
        {
            return workerAgentList.Select(p=>new Domain.Models.Agent()
            {
                AgentName = p.Agent.AgentName,
                AgentTitle = p.Agent.AgentTitle,
                Team = p.Agent.Team
            }).ToList();
        }
    }
}