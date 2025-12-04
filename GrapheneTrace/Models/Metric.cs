using System;

namespace GrapheneTrace.Models
{
    public class Metric
    {
        public int Id { get; set; }

        public int FrameId { get; set; }
        public DateTime Timestamp { get; set; }

        public double Temperature { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
    }
}
