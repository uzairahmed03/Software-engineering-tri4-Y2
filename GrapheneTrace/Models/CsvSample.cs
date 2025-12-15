using System;

namespace GrapheneTrace.Models
{
    /// <summary>
    /// Stores a single value from a CSV file at a specific row and column.
    /// Filename supplies SourceId + MeasureDate; CSV supplies numeric columns (0..31).
    /// </summary>
    public class CsvSample
    {
        public long Id { get; set; }

        // From filename prefix (e.g. "1c0fd777")
        public string SourceId { get; set; } = string.Empty;

        // From filename date portion (e.g. 2025-10-11)
        public DateTime MeasureDate { get; set; }

        // 0-based row index in the CSV (line number)
        public int RowIndex { get; set; }

        // 0..31
        public int ColIndex { get; set; }

        public double Value { get; set; }
    }
}
