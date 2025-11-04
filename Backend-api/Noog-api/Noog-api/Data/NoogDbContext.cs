using Microsoft.EntityFrameworkCore;
using Noog_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Noog_api.Models.Application;
using Noog_api.Models.AssemblyAi;

namespace Noog_api.Data
{
    public class NoogDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public NoogDbContext(DbContextOptions<NoogDbContext> options) : base(options)
        {
        }

        // Application
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<ProjectGroupUser> ProjectGroupUsers { get; set; }
        public DbSet<RecentGroupActivity> RecentGroupActivities { get; set; }
        public DbSet<GroupMeeting> GroupMeetings { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<GroupStorage> GroupStorages { get; set; }

        // AssemblyAI
        public DbSet<Transcript> Transcripts { get; set; }

        // OpenAI
        public DbSet<Summary> Summaries { get; set; }

        // Identity
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Application

            modelBuilder.Entity<ProjectGroup>(entity =>
            {
                // TODO - Fluent API 

                // Primary key
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ProjectGroupUser>(entity =>
            {
                // TODO - Fluent API
            });

            modelBuilder.Entity<RecentGroupActivity>(entity =>
            {
                // TODO - Fluent API 

                // Primary key
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<GroupMeeting>(entity =>
            {
                // TODO - Fluent API 

                // Primary key
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<GroupChat>(entity =>
            {
                // TODO - Fluent API 

                // Primary key
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<GroupStorage>(entity =>
            {
                // TODO - Fluent API 

                // Primary key
                entity.HasKey(e => e.Id);
            });

            // AssemblyAI

            modelBuilder.Entity<Transcript>(entity =>
            {
                // TODO - Fluent API 

                // Primary key
                entity.HasKey(e => e.Id);
            });

            // OpenAI

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
                    CreatedAt = new DateTime(2025, 10, 13, 12, 30, 0, DateTimeKind.Utc)
                },
                new Summary
                {
                    SummaryId = 2,
                    Title = "Second Summary",
                    Content = "This is the content of the second summary.",
                    CreatedAt = new DateTime(2025, 10, 14, 8, 15, 0, DateTimeKind.Utc)
                }
            );

            // Identity

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.ImgProfile)
                .HasDefaultValue("https://t3.ftcdn.net/jpg/06/33/54/78/240_F_633547842_AugYzexTpMJ9z1YcpTKUBoqBF0CUCk10.jpg");

        }

        // Handles modificationAt and creationAt of an entity if it inherits BaseEntity
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            // Todo - change to switch statement if we want to handle soft deletes here

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                entry.Entity.ModifiedAt = DateTime.UtcNow;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
