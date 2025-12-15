using GrapheneTrace.Data;
using GrapheneTrace.Models;

namespace GrapheneTrace.Services
{
    public class FrameParserService
    {
        private readonly ApplicationDbContext _db;

        public FrameParserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Metric> ProcessFrame(int userId, string rawData)
        {
            var frame = new Frame
            {
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                RawData = rawData
            };

            _db.Frames.Add(frame);
            await _db.SaveChangesAsync();

            var metric = ParseMetrics(frame.Id, rawData);

            _db.Metrics.Add(metric);
            await _db.SaveChangesAsync();

            return metric;
        }

        private Metric ParseMetrics(int frameId, string rawData)
        {
            var parts = rawData.Split(',');

            return new Metric
            {
                FrameId = frameId,
                Timestamp = DateTime.UtcNow,
                Temperature = double.Parse(parts[0]),
                Voltage = double.Parse(parts[1]),
                Current = double.Parse(parts[2])
            };
        }
    }
}
