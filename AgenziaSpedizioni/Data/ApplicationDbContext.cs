using AgenziaSpedizioni.Models;
using Microsoft.EntityFrameworkCore;

namespace Spedizioni.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<Spedizione>()
                .Property(s => s.CostoSpedizione)
                .HasColumnType("decimal(18, 2)");
        }
        public DbSet<AgenziaSpedizioni.Models.Spedizione> Spedizioni { get; set; } = default!;
        public DbSet<AgenziaSpedizioni.Models.DettagliSpedizione> DettagliSpedizioni { get; set; } = default!;
    }
}
