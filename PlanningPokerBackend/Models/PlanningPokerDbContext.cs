using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlanningPokerBackend.Models
{
    public class PlanningPokerDbContext : DbContext
    {
        public PlanningPokerDbContext(DbContextOptions<PlanningPokerDbContext> options) 
            :base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<PlayTable> PlayTables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayTable>()
                .HasMany(pt => pt.Participants)
                .WithOne(u => u.PlayTable);

            base.OnModelCreating(modelBuilder);
        }
    }
}
