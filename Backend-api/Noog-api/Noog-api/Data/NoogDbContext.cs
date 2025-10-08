using Microsoft.EntityFrameworkCore;
using Noog_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Noog_api.Data
{
    public class NoogDbContext : IdentityDbContext<ApplicationUser>
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
                      .HasMaxLength(5000)
                      .HasColumnType("nvarchar(5000)");

                entity.Property(e => e.CreatedAt)
                        .IsRequired()
                        .HasColumnType("datetime2");
            });
        }

        public DbSet<Summary> Summaries { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

    }
}
