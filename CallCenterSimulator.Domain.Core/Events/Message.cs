using MediatR;

namespace CallCenterSimulator.Domain.Core
{
    public abstract class Message : IRequest<bool>
    {
        public string MessageType { get; protected set; }

        protected Message()
        {
            this.MessageType = GetType().Name;
        }
    }
}
