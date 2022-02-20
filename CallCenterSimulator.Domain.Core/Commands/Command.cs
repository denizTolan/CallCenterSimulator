using System;

namespace CallCenterSimulator.Domain.Core
{
    public abstract class Command : Message
    {
        public DateTime TimeStamp { get; protected set; }

        protected Command()
        {
            this.TimeStamp = DateTime.Now;
        }
    }
}
