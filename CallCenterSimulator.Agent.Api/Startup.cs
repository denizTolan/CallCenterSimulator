using CallCenterSimulator.Agent.Api.Service.HostedServices;
using CallCenterSimulator.Agent.Application.Service;
using CallCenterSimulator.Agent.Application.Service.Interface;
using CallCenterSimulator.Agent.Data.Context;
using CallCenterSimulator.Agent.Data.Repository;
using CallCenterSimulator.Agent.Domain.Data;
using CallCenterSimulator.Agent.Domain.EventHandlers;
using CallCenterSimulator.Agent.Domain.Events;
using CallCenterSimulator.Agent.Domain.Interface;
using CallCenterSimulator.Agent.Domain.Interfaces;
using CallCenterSimulator.Bus;
using CallCenterSimulator.Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CallCenterSimulator.Agent.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CallCenterSimulatorDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("CallCenterSimulatorDbConnection"));
                },ServiceLifetime.Singleton
            );
            
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            services.AddMemoryCache();

            services.AddTransient<IAgentRepository, AgentRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddSingleton<IAgentService, AgentService>();
            services.AddSingleton<CachedAgentService>();
                
            //Subscriptions
            services.AddTransient<TransferEventHandler>();
            services.AddTransient<AgentEventHandler>();
            
            services.AddSingleton<IUserData,UserData>();

            //Domain Events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();
            services.AddTransient<IEventHandler<AgentCreatedEvent>, AgentEventHandler>();

            services.AddHostedService<CallbackAgentService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CallCenterSimulator.Agent.Api", Version = "v1"});
            });
            
            
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CallCenterSimulator.Agent.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            ConfigureEventBus(app);
        }
        
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
        }
    }
}