using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noog_api.Models
{
    public abstract class BaseEntity
    {
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedAt { get; set; }

        // Optional fields but for soft deletes and inactive users etc
        [Column(TypeName = "datetime2")]
        public DateTime? ArchivedAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DeletedAt { get; set; }
    }
}
