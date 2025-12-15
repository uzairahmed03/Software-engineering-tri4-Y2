using System;
using System.Linq;
using System.Threading.Tasks;
using GrapheneTrace.Data;
using GrapheneTrace.Models;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class MetricsService
    {
        private readonly ApplicationDbContext _db;

        public MetricsService(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns the most recent metrics entry (used by dashboards).
        /// </summary>
        public async Task<Metric?> GetLatestMetric()
        {
            return await _db.Metrics
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns metrics over a date range (used by clinician dashboard).
        /// </summary>
        public async Task<Metric[]> GetMetricsInRange(DateTime from, DateTime to)
        {
            return await _db.Metrics
                .Where(m => m.Timestamp >= from && m.Timestamp <= to)
                .OrderBy(m => m.Timestamp)
                .ToArrayAsync();
        }

        /// <summary>
        /// Derives metrics from CSV data and stores them.
        /// Safe to run multiple times (idempotent per day).
        /// </summary>
        public async Task GenerateMetricsFromCsvAsync(
            string sourceId,
            DateTime date)
        {
            var day = date.Date;

            // Avoid duplicates
            bool alreadyExists = await _db.Metrics
                .AnyAsync(m => m.Timestamp.Date == day);

            if (alreadyExists)
                return;

            var q = _db.CsvSamples.AsNoTracking()
                .Where(x =>
                    x.SourceId == sourceId &&
                    x.MeasureDate == day);

            if (!await q.AnyAsync())
                return;

            const double CONTACT_THRESHOLD = 0.01;

            double peakPressure = await q.MaxAsync(x => x.Value);

            int contactArea = await q
                .Where(x => x.Value > CONTACT_THRESHOLD)
                .Select(x => new { x.RowIndex, x.ColIndex })
                .Distinct()
                .CountAsync();

            int frameCount = await q
                .Select(x => x.RowIndex)
                .Distinct()
                .CountAsync();

            var metric = new Metric
            {
                PeakPressure = Math.Round(peakPressure, 2),
                ContactArea = contactArea,
                FrameCount = frameCount,
                Timestamp = day
            };

            _db.Metrics.Add(metric);
            await _db.SaveChangesAsync();
        }
    }
}
