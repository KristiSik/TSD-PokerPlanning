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
    }
}
