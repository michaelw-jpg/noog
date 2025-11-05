using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models.Application;

namespace Noog_api.Data.Configurations.Application
{
    public class GroupMeetingConfig : IEntityTypeConfiguration<GroupMeeting>
    {
        public void Configure(EntityTypeBuilder<GroupMeeting> b)
        {
            // Primary key
            b.ToTable("GroupMeeting");
            b.HasKey(e => e.Id);

            // The StreamIO generated callId
            b.Property(e => e.CallId)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            // One-to-Many relationship
            b.HasMany(e => e.Transcripts)
                .WithOne(t => t.GroupMeeting);

            // Navigation is set in Projectgroupconfig
        }
    }
}
