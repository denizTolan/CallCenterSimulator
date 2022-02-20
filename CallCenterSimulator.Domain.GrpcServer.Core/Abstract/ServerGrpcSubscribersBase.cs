using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenterSimulator.Domain.GrpcServer.Core.Model;
using Microsoft.Extensions.Logging;

namespace CallCenterSimulator.Domain.GrpcServer.Core.Abstract
{
    public class ServerGrpcSubscribersBase<TResponse>
    {
        private readonly ConcurrentDictionary<string, SubscribersModel<TResponse>> _subscribers = 
            new ConcurrentDictionary<string, SubscribersModel<TResponse>>();

        private ILogger Logger { get; set; }

        public ServerGrpcSubscribersBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<ServerGrpcSubscribersBase<TResponse>> ();
        }

        public async Task BroadcastMessageAsync(TResponse message)
        {
            await BroadcastMessages(message);
        }

        public Dictionary<string, SubscribersModel<TResponse>> GetSubscriberList()
        {
            return _subscribers.ToDictionary(p=>p.Key,p=>p.Value);
        }

        public void AddSubscriber(SubscribersModel<TResponse> subscriber)
        {
            bool added = _subscribers.TryAdd(subscriber.Id, subscriber);
            
           
            
            Logger.LogInformation($"Kullanıcı register yapıldı ID: {subscriber.Id}");
            if (!added)
                Logger.LogInformation($"Kullanıcı register olamadı ID: {subscriber.Id}");
        }

        public void RemoveSubscriber(SubscribersModel<TResponse> subscriber)
        {
            try
            {
                _subscribers.TryRemove(subscriber.Id, out SubscribersModel<TResponse> item);
                Logger.LogInformation($"Kullanıcı Silindi: {item.Id}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Kullanıcı Silinemedi {subscriber.Id}");
            }
        }

        public virtual bool Filter(SubscribersModel<TResponse> subscriber, TResponse message) => true;
        
        private async Task BroadcastMessages(TResponse message)
        {
            foreach (var subscriber in _subscribers.Values)
            {
                var item = await SendMessageToSubscriber(subscriber, message);
                if (item != null)
                    RemoveSubscriber(item);
            }
        }

        private async Task<SubscribersModel<TResponse>> SendMessageToSubscriber(SubscribersModel<TResponse> subscriber, TResponse message)
        {
            try
            {
                if (Filter(subscriber, message))
                {
                    Logger.LogInformation($"Mesaj Gönderiliyor: {message}");
                    await subscriber.Subscriber.WriteAsync(message);
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Mesaj gönderilemedi.");
                return subscriber;
            }
        }
    }
}