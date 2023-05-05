using Microsoft.EntityFrameworkCore;
namespace RentalOfPremises.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<PhysicalEntity> PhysicalEntities { get; set; } = null!;
        public DbSet<Placement> Placements { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhysicalEntity>()
                .HasOne(pe => pe.User)
                .WithOne(u => u.PhysicalEntity)
                .HasForeignKey<PhysicalEntity>(pe => pe.UserId);

            modelBuilder.Entity<Placement>()
                .HasOne(p => p.PhysicalEntity)
                .WithMany(pe => pe.Placements)
                .HasForeignKey(p => p.PhysicalEntityId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
