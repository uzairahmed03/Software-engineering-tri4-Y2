using System;
using System.ComponentModel.DataAnnotations;

namespace GrapheneTrace.Models
{
    public class Metric
    {
        [Key]
        public int Id { get; set; }

        // =========================
        // EXISTING FIELDS (DO NOT REMOVE)
        // Used by FrameParserService
        // =========================
        public int FrameId { get; set; }

        public double Temperature { get; set; }

        public double Voltage { get; set; }

        public double Current { get; set; }

        // =========================
        // NEW CSV-DERIVED METRICS
        // =========================
        public double PeakPressure { get; set; }

        public int ContactArea { get; set; }

        public int FrameCount { get; set; }

        // =========================
        // TIMESTAMP (USED BY DASHBOARDS)
        // =========================
        public DateTime Timestamp { get; set; }
    }
}
