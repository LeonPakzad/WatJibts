using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Models

{
    public class WatDbContext : DbContext
    {
        public WatDbContext(DbContextOptions<WatDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;
    }
}