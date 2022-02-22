using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CallCenterSimulator.Agent.Application.Service.Interface;
using Microsoft.Extensions.Hosting;

namespace CallCenterSimulator.Agent.Api.Service.HostedServices
{
    public class CallbackAgentService : BackgroundService
    {
        private readonly IAgentService _agentService;
        
        public CallbackAgentService(IAgentService agentService)
        {
            this._agentService = agentService;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await this._agentService.CalculateAvailableAgent();
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}