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

        public async Task<Metric> GetLatestMetric()
        {
            return await _db.Metrics
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Metric>> GetMetricsInRange(DateTime start, DateTime end)
        {
            return await _db.Metrics
                .Where(m => m.Timestamp >= start && m.Timestamp <= end)
                .ToListAsync();
        }
    }
}
