using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace src.Data
{
    public class WatDbContext : IdentityDbContext
    {
        public WatDbContext(DbContextOptions<WatDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;
        public DbSet<Location> Location { get; set; } = null!;
        public DbSet<LunchTime> LunchTime { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}