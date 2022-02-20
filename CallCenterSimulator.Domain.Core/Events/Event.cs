using System;

namespace CallCenterSimulator.Domain.Core
{
    public abstract class Event
    {
        public DateTime TimeStamp { get; protected set; }

        protected Event()
        {
            this.TimeStamp = DateTime.Now;
        }
    }
}
