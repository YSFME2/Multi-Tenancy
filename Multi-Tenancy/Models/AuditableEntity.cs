using Multi_Tenancy.Models.Interfaces;

namespace Multi_Tenancy.Models
{
    public class AuditableEntity : IAuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set ; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
