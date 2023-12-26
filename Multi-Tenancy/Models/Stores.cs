using System.ComponentModel.DataAnnotations;

namespace Multi_Tenancy.Models
{
    public class Stores : AuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
