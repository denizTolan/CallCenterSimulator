using System;
using System.Collections.Generic;
using System.Linq;
using CallCenterSimulator.Agent.Data.Context;
using CallCenterSimulator.Agent.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace CallCenterSimulator.Agent.Application.Service
{
    public class CachedAgentService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CallCenterSimulatorDbContext _callCenterSimulatorDbContext;
        
        public CachedAgentService(IMemoryCache memoryCache,
            CallCenterSimulatorDbContext callCenterSimulatorDbContext)
        {
            this._memoryCache = memoryCache;
            this._callCenterSimulatorDbContext = callCenterSimulatorDbContext;
        }

        public List<Domain.Models.Agent> GetCachedAgentList()
        {
            if (!_memoryCache.TryGetValue(CacheKey.Agent, out List<Domain.Models.Agent> cachedAgentList))
            {
                var agentList = this._callCenterSimulatorDbContext.Agents.ToList();
                
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _memoryCache.Set(CacheKey.Agent, agentList, cacheEntryOptions);

                cachedAgentList = new List<Domain.Models.Agent>();
                cachedAgentList.AddRange(agentList);
            }

            return cachedAgentList;
        }
    }
}