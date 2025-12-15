using System;

namespace GrapheneTrace.Models
{
    public class Alert
    {
        public int Id { get; set; }

        public int FrameId { get; set; }
        public string Message { get; set; }

        public DateTime Timestamp { get; set; }
        public bool IsAcknowledged { get; set; }
    }
}
