using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models.Application;

namespace Noog_api.Data.Configurations.Application
{
    public class GroupStorageConfig : IEntityTypeConfiguration<GroupStorage>
    {
        public void Configure(EntityTypeBuilder<GroupStorage> b)
        {
            // Primary key
            b.ToTable("GroupStorage");
            b.HasKey(e => e.Id);

            // Many to one relationship
            b.HasMany(e => e.Summaries)
                .WithOne(s => s.GroupStorage);

            // Navigation is set in Projectgroupconfig
        }
    }
}
