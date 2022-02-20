using System;
using CallCenterSimulator.Domain.GrpcServer.Core.Abstract;
using Microsoft.Extensions.Logging;
using NotificationGrpcService;

namespace CallCenterSimulator.Domain.GrpcServer.Core
{
    public class MessageProcessor : MessageProcessorBase<RegisterRequest, RegisterResponse>
    {
        public MessageProcessor(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public override string GetClientId(RegisterRequest message) => message.ClientId;

        public override RegisterResponse Process(RegisterRequest message)
        {
            if (string.IsNullOrEmpty(message.Payload))
                return null;

            Logger.LogInformation($"To be processed: {message}");

            //
            // Request message processing should be placed here
            //

            if (message.Response != ResponseType.Required)
                return null;
            
            var timestamp = DateTime.UtcNow.Ticks;

            try
            {
                return new RegisterResponse()
                {
                    ClientId = message.ClientId,
                    MessageId = message.MessageId,
                    Type = message.Type,
                    Time = timestamp,
                    Payload = $"Response to \"{message.Payload}\"",
                    Status = MessageStatus.Processed,
                };
            }
            catch (Exception e)
            {
                return new RegisterResponse
                {
                    ClientId = message.ClientId,
                    MessageId = message.MessageId,
                    Type = message.Type,
                    Time = timestamp,
                    Payload = e.Message,
                    Status = MessageStatus.Error,
                };
            }
        }
    }
}