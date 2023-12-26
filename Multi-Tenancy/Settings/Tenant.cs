namespace Multi_Tenancy.Settings
{
    public class Tenant
    {
        public string TenantId { get; set; } = null!;
        public string TenantName { get; set; } = null!;
        public string? DbConnection { get; set; }
        public string? DbProvider { get; set; }
    }
}
