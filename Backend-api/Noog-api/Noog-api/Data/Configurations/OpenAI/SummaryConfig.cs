using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noog_api.Models;

namespace Noog_api.Data.Configurations.OpenAI
{
    public class SummaryConfig : IEntityTypeConfiguration<Summary>
    {
        public void Configure(EntityTypeBuilder<Summary> b)
        {
            // Primary key
            b.HasKey(e => e.SummaryId);

            // Auto-incrementing ID
            b.Property(e => e.SummaryId)
                  .ValueGeneratedOnAdd()
                  .HasColumnType("int");



            // sets max length 100 and makes it required
            b.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnType("nvarchar(100)");

            b.Property(e => e.Content)
                  .IsRequired()
                  .HasMaxLength(4000)
                  .HasColumnType("nvarchar(4000)");

            b.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");
        }
    }
}
