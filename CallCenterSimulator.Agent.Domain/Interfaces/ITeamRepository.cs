using System.Collections.Generic;
using CallCenterSimulator.Agent.Domain.Models;

namespace CallCenterSimulator.Agent.Domain.Interfaces
{
    public interface ITeamRepository
    {
        List<Team> GetAllTeam();
    }
}