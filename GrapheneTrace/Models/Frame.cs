using System;

namespace GrapheneTrace.Models
{
    public class Frame
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public DateTime Timestamp { get; set; }
        public string RawData { get; set; }
    }
}
