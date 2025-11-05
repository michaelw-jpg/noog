using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models.Application;

namespace Noog_api.Data.Configurations.Application
{
    public class ProjectGroupConfig : IEntityTypeConfiguration<ProjectGroup>
    {
        public void Configure(EntityTypeBuilder<ProjectGroup> b)
        {
            // Primary Key
            b.ToTable("ProjectGroups");
            b.HasKey(e => e.Id);

            // Values
            b.Property(e => e.Name).IsRequired().HasMaxLength(50);
            b.Property(e => e.ImageUrl).IsRequired().HasMaxLength(250); // Url

            // One-to-one relationships 
            b.HasOne(e => e.GroupMeeting)
                .WithOne(gm => gm.ProjectGroup)
                .HasForeignKey<GroupMeeting>(gm => gm.ProjectGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.GroupChat)
                .WithOne(gc => gc.ProjectGroup)
                .HasForeignKey<GroupChat>(gc => gc.ProjectGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.GroupStorage)
                .WithOne(gs => gs.ProjectGroup)
                .HasForeignKey<GroupStorage>(gs => gs.ProjectGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many relationships
            b.HasMany(e => e.ProjectGroupUsers)
                .WithOne(pgu => pgu.ProjectGroup);

            b.HasMany(e => e.RecentGroupActivities)
                .WithOne(rga => rga.ProjectGroup);
        }
    }
}
