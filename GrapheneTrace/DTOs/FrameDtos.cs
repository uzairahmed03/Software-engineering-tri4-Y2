using System;

namespace GrapheneTrace.DTOs
{
    public class FrameUploadDto
    {
        public int UserId { get; set; }
        public string RawData { get; set; }
    }

    public class FrameResponseDto
    {
        public int FrameId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
    }
}
