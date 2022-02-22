using System.Threading.Tasks;
using CallCenterSimulator.Agent.Application.Model;
using CallCenterSimulator.Agent.Domain.Data;
using CallCenterSimulator.Agent.Domain.Events;

namespace CallCenterSimulator.Agent.Application.Service.Interface
{
    public interface IAgentService
    {
        Domain.Models.Agent GetAvailableAgent();

        WorkerAgent AddWorkerAgent(TransferCreatedEvent transferCreatedEvent);

        Task CalculateAvailableAgent();
    }
}