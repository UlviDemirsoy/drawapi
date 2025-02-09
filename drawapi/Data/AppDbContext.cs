using drawapi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace drawapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Draw> Draws { get; set; }
        public DbSet<GroupTeam> GroupTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Team>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Team>()
                .HasIndex(t => new { t.Name, t.Country })
                .IsUnique();

            modelBuilder.Entity<Team>()
                .Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .Property(t => t.Country)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Draw)       
                .WithMany(d => d.Groups)    
                .HasForeignKey(g => g.DrawId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupTeam>()
                .HasKey(gt => new { gt.GroupId, gt.TeamId });

            modelBuilder.Entity<GroupTeam>()
                .HasOne(gt => gt.Group)
                .WithMany(g => g.GroupTeams)
                .HasForeignKey(gt => gt.GroupId);

            modelBuilder.Entity<GroupTeam>()
                .HasOne(gt => gt.Team)
                .WithMany(t => t.GroupTeams)
                .HasForeignKey(gt => gt.TeamId);


        }
    }
}
