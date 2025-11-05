using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models.Application;

namespace Noog_api.Data.Configurations.Application
{
    public class GroupChatConfig : IEntityTypeConfiguration<GroupChat>
    {
        public void Configure(EntityTypeBuilder<GroupChat> b)
        {
            // Primary key
            b.ToTable("GroupChat");
            b.HasKey(e => e.Id);

            // To be continued...

            // Navigation is set in Projectgroupconfig
        }
    }
}
