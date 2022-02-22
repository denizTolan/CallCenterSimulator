using CallCenterSimulator.Bus;
using CallCenterSimulator.Client.Api.Services;
using CallCenterSimulator.Client.Domain.CommandHandlers;
using CallCenterSimulator.Client.Domain.Commands;
using CallCenterSimulator.Client.Domain.EventHandlers;
using CallCenterSimulator.Client.Domain.Events;
using CallCenterSimulator.Domain.Core;
using CallCenterSimulator.Domain.GrpcServer.Core;
using MediatR;
using MicroRabbit.Transfer.Domain.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CallCenterSimulator.Client.Api
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
            services.AddSingleton<ServerGrpcSubscribers>();
            services.AddSingleton<MessageProcessor>();
            
            //Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            //Subscriptions
            services.AddTransient<TransferEventHandler>();

            //Domain Events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            //Domain Banking Commands
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CallCenterSimulator.Client.Api", Version = "v1"});
            });
            
            services.AddMediatR(typeof(Startup));
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CallCenterSimulator.Client.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<NotificationService>();
            });

            ConfigureEventBus(app);
        }
        
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<AgentCreatedEvent, AgentEventHandler>();
        }
    }
}