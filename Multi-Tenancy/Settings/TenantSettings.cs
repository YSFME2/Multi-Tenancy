namespace Multi_Tenancy.Settings
{
    public class TenantSettings
    {
        public TenantConfiguration DefaultConfiguration { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
}
