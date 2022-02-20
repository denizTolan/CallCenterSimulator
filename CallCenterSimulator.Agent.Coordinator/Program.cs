using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CallCenterSimulator.Bus;
using CallCenterSimulator.Domain.Core;
using MediatR;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CallCenterSimulator.Agent.Coordinator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<Form1>();
                    services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
                    {
                        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                        return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
                    });
                });

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var eventBus = services.GetRequiredService<IEventBus>();
                    eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
                    
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    var form1 = services.GetService<Form1>();
                    Application.Run(form1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
        }
        
        
        
    }
}