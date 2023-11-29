using Daily_Metting.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Daily_Metting.Data
{
    public class DailyMeetingDbContext : IdentityDbContext
    {
        public DailyMeetingDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User>? Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Value> Values { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<APU> APUs { get; set; }
        public DbSet<Attainement> Attainements { get; set; }

    }
}
