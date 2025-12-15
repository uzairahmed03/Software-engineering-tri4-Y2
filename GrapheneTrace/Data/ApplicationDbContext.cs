using GrapheneTrace.Models;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public DbSet<CsvSample> CsvSamples { get; set; }
    }
}
