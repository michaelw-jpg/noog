using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models.Application;

namespace Noog_api.Data.Configurations.Application
{
    public class RecentGroupActivityConfig : IEntityTypeConfiguration<RecentGroupActivity>
    {
        public void Configure(EntityTypeBuilder<RecentGroupActivity> b)
        {
            // Primary key
            b.ToTable("RecentGroupActivities");
            b.HasKey(e => e.Id);

            // Title
            b.Property(e => e.Title).HasMaxLength(100).IsRequired();
            // ENUM Meeting, Chat, Storage
            b.Property(e => e.SourceType).HasConversion<string>().HasMaxLength(20).IsRequired();

            // One-to-Many Relationship
            b.HasOne(e => e.ProjectGroup)
                .WithMany(pg => pg.RecentGroupActivities)
                .HasForeignKey(e => e.ProjectGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
