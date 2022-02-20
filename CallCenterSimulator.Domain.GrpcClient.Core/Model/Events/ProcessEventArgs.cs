using System;

namespace CallCenterSimulator.Domain.GrpcClient.Core.Model.Events
{
    public class ProcessEventArgs : EventArgs
    {
        public bool IsSuccessful { get; set; }
        public DateTime CompletionTime { get; set; }
    }
}