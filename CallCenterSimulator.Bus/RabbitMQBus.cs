using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallCenterSimulator.Domain.Core;

namespace CallCenterSimulator.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            this._mediator = mediator;
            this._handlers = new Dictionary<string, List<Type>>();
            this._eventTypes = new List<Type>();
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName,false,false,false,null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("",eventName,null,body);
            }
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName,new List<Type>());
            }

            if (_handlers[eventName].Any(p => p.GetType() == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} aldready is registered for {eventName}");
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsumer<T>();
        }

        private void StartBasicConsumer<T>() 
            where T : Event
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost",
                DispatchConsumersAsync=true 
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName,false,false,false,null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());

            try
            {
                await ProcessorEvet(eventName,message).ConfigureAwait(false);
            }
            catch (Exception exp)
            {
            }
        }

        private async Task ProcessorEvet(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = this._serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null)
                            continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreateType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreateType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
    }
}
