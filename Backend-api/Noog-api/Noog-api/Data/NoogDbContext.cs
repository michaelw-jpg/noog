using Microsoft.EntityFrameworkCore;
using Noog_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Noog_api.Models.Application;
using Noog_api.Models.AssemblyAi;
using System.Reflection;

namespace Noog_api.Data
{
    /// <summary>
    /// Please feel free to annotate however you want. I introduced Configurations folder within the data folder
    /// to handle all Fluent API (Data annotations and such).
    /// Modelbuilder could be used for some seeding-testing and once established, moved to configuration.
    /// </summary>
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

            // Applies all configurations for our Entities 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            // AssemblyAI

            modelBuilder.Entity<Transcript>(entity =>
            {
                // TODO - Fluent API / DataAnnotations / IEntityTypeConfiguration<Transcript>

                // Primary key
                entity.HasKey(e => e.Id);
            });

            // Identity

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.ImgProfile)
                .HasDefaultValue("https://t3.ftcdn.net/jpg/06/33/54/78/240_F_633547842_AugYzexTpMJ9z1YcpTKUBoqBF0CUCk10.jpg");

            // OpenAI

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
        }

        // Handles modificationAt and creationAt of an entity if it inherits BaseEntity
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            // Todo - change to switch statement if we want to handle deletedAt and/or archivedAt here

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
