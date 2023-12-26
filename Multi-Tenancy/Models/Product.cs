using Multi_Tenancy.Models.Interfaces;

namespace Multi_Tenancy.Models
{
    public class Product : AuditableEntity,IHaveTenant
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
