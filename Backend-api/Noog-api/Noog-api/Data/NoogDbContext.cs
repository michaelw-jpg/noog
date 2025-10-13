using Microsoft.EntityFrameworkCore;
using Noog_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Noog_api.Data
{
    public class NoogDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public NoogDbContext(DbContextOptions<NoogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Summary>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.SummaryId);

                // Auto-incrementing ID
                entity.Property(e => e.SummaryId)
                      .ValueGeneratedOnAdd()
                      .HasColumnType("int");



                // sets max length 100 and makes it required
                entity.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnType("nvarchar(100)");

                entity.Property(e => e.Content)
                      .IsRequired()
                      .HasMaxLength(4000)
                      .HasColumnType("nvarchar(4000)");

                entity.Property(e => e.CreatedAt)
                        .IsRequired()
                        .HasColumnType("datetime2");
            });
            modelBuilder.Entity<Summary>().HasData(
                new Summary
                {
                    SummaryId = 1,
                    Title = "First Summary",
                    Content = "This is the content of the first summary.",
                    CreatedAt = DateTime.UtcNow
                },
                new Summary
                {
                    SummaryId = 2,
                    Title = "Second Summary",
                    Content = "This is the content of the second summary.",
                    CreatedAt = DateTime.UtcNow
                }
                );

        }

        public DbSet<Summary> Summaries { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }

    }
}
