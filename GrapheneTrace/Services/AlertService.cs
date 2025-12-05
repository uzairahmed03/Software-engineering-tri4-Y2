using GrapheneTrace.Data;
using GrapheneTrace.Models;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class AlertService
    {
        private readonly ApplicationDbContext _db;

        public AlertService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Alert>> GetAlerts()
        {
            return await _db.Alerts.ToListAsync();
        }

        public async Task<Alert> AddAlert(int frameId, string message)
        {
            var alert = new Alert
            {
                FrameId = frameId,
                Message = message,
                Timestamp = DateTime.UtcNow,
                IsAcknowledged = false
            };

            _db.Alerts.Add(alert);
            await _db.SaveChangesAsync();

            return alert;
        }

        public async Task<bool> AcknowledgeAlert(int id)
        {
            var alert = await _db.Alerts.FindAsync(id);

            if (alert == null) return false;

            alert.IsAcknowledged = true;
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
