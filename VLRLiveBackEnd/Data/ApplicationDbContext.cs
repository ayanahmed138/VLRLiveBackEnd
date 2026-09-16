using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using VLRLiveBackEnd.Models;

namespace VLRLiveBackEnd.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Define your DbSets here, for example:
        // public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<Team> Teams { get; set; } = null!;
    }
}
