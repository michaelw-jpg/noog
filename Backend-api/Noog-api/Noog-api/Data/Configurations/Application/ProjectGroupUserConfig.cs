using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models.Application;

namespace Noog_api.Data.Configurations.Application
{
    public class ProjectGroupUserConfig : IEntityTypeConfiguration<ProjectGroupUser>
    {
        public void Configure(EntityTypeBuilder<ProjectGroupUser> b)
        {
            // Composite Key
            b.ToTable("ProjectGroupUser");
            b.HasKey(e => new { e.ProjectGroupId, e.ApplicationUserId });

            // Boolean, might change to role enum (User, admin, owner)
            b.Property(e => e.IsAdmin).IsRequired();
            // b.Property(e => e.Role).HasConversion<string>().HasMaxLength(20).IsRequired()

            // One-to-Many Relationships
            b.HasOne(e => e.ProjectGroup)
                .WithMany(pg => pg.ProjectGroupUsers)
                .HasForeignKey(e => e.ProjectGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.ApplicationUser)
                .WithMany(au => au.ProjectGroupUsers)
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
