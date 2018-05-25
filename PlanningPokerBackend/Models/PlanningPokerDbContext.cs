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
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayTable>()
                .HasMany(pt => pt.Participants)
                .WithOne(u => u.PlayTable);

            modelBuilder.Entity<PlayTable>()
                .HasOne(pt => pt.Admin);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Inviter);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Participant);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.PlayTable);

            modelBuilder.Entity<PlayTable>()
                .HasOne(pt => pt.CurrentGame)
                .WithOne(g => g.PlayTable);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Answers);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User);

            base.OnModelCreating(modelBuilder);
        }
    }
}
