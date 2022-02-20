using System;

namespace CallCenterSimulator.Client.Domain.Models
{
    public class User
    {
        public Guid TransactionId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}