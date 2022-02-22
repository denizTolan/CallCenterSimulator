using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CallCenterSimulator.Agent.Application.Extenstions;
using CallCenterSimulator.Agent.Application.Model;
using CallCenterSimulator.Agent.Application.Service.Interface;
using CallCenterSimulator.Agent.Data.Context;
using CallCenterSimulator.Agent.Domain.Commands;
using CallCenterSimulator.Agent.Domain.Common;
using CallCenterSimulator.Agent.Domain.Events;
using CallCenterSimulator.Agent.Domain.Interface;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Agent.Application.Service
{
    public class AgentService : IAgentService
    {
        private readonly CallCenterSimulatorDbContext _callCenterSimulatorDbContext;
        private readonly CachedAgentService _cachedAgentService;
        private readonly List<WorkerAgent> _workerList;
        private readonly IUserData _userData;
        private readonly IEventBus _bus;

        public AgentService(CallCenterSimulatorDbContext callCenterSimulatorDbContext
            ,CachedAgentService cachedAgentService
            ,IUserData userData
            ,IEventBus bus)
        {
            this._callCenterSimulatorDbContext = callCenterSimulatorDbContext;
            this._cachedAgentService = cachedAgentService;
            this._workerList = new List<WorkerAgent>();
            this._userData = userData;
            this._bus = bus;
        }
        
        public Domain.Models.Agent GetAvailableAgent()
        {
            var allAgentList = this._cachedAgentService.GetCachedAgentList();

            var workingAgentList = allAgentList.Where(p => p.Team?.ShiftStartTime <= DateTimeOffset.UtcNow.TimeOfDay
                                    && p.Team?.ShifEndTime >= DateTimeOffset.UtcNow.TimeOfDay).ToList();

            var freeAgentList = allAgentList.Except(_workerList.ToAgentList()).OrderBy(p=>(int)p.AgentTitle).ToList();

            return freeAgentList.FirstOrDefault();
        }

        public WorkerAgent AddWorkerAgent(TransferCreatedEvent transferCreatedEvent)
        {
            var foundedAvailableAgent = this.GetAvailableAgent();

            var workerAgent = new WorkerAgent()
            {
                Agent = foundedAvailableAgent,
                TransactionId = transferCreatedEvent.TransactionId,
                UserName = transferCreatedEvent.UserName
            };
            
            this._workerList.Add(workerAgent);

            return workerAgent;
        }

        public async Task CalculateAvailableAgent()
        {
            foreach (var userData in this._userData.PopUserDataList())
            {
                var addedWorkerAgent = this.AddWorkerAgent(userData);
            
                var createTransferCommand = new CreateAgentCommand(
                    userData.TransactionId,
                    userData.UserName,
                    userData.CreatedTime,
                    addedWorkerAgent.Agent.Id,
                    addedWorkerAgent.UserName
                );

                _bus.SendCommand(createTransferCommand);
            }

            await Task.FromResult(true);
        }
    }
}