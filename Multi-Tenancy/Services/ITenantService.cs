
namespace Multi_Tenancy.Services
{
    public interface ITenantService
    {
        string GetDbConnection();
        Tenant? GetCurrentTenant();
        string GetDbProvider();
    }
}
