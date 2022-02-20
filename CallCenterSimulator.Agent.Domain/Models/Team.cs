using System;

namespace CallCenterSimulator.Agent.Domain.Models
{
    public class Team : EntityBase
    {
        public string TeamName { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShifEndTime { get; set; }
        public int ShiftOrder { get; set; }
    }
}