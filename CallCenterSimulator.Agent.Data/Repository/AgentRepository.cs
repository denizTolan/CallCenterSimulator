using System.Collections.Generic;
using System.Linq;
using CallCenterSimulator.Agent.Data.Context;
using CallCenterSimulator.Agent.Domain.Interfaces;

namespace CallCenterSimulator.Agent.Data.Repository
{
    public class AgentRepository : IAgentRepository
    {
        private readonly CallCenterSimulatorDbContext _callCenterSimulatorDbContext;
        
        public AgentRepository(CallCenterSimulatorDbContext callCenterSimulatorDbContext)
        {
            this._callCenterSimulatorDbContext = callCenterSimulatorDbContext;
        }
        
        public List<Domain.Models.Agent> GetAllAgents()
        {
            return this._callCenterSimulatorDbContext.Agents.ToList();
        }
    }
}