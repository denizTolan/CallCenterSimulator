using System.Collections.Generic;

namespace CallCenterSimulator.Agent.Domain.Interfaces
{
    public interface IAgentRepository
    {
        List<Models.Agent> GetAllAgents();
    }
}