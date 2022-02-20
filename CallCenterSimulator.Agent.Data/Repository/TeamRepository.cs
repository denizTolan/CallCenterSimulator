using System.Collections.Generic;
using System.Linq;
using CallCenterSimulator.Agent.Data.Context;
using CallCenterSimulator.Agent.Domain.Interfaces;
using CallCenterSimulator.Agent.Domain.Models;

namespace CallCenterSimulator.Agent.Data.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly CallCenterSimulatorDbContext _callCenterSimulatorDbContext;
        
        public TeamRepository(CallCenterSimulatorDbContext callCenterSimulatorDbContext)
        {
            this._callCenterSimulatorDbContext = callCenterSimulatorDbContext;
        }

        public List<Team> GetAllTeam()
        {
            return _callCenterSimulatorDbContext.Teams.ToList();
        }
    }
}