using Microsoft.Extensions.Options;

namespace Multi_Tenancy.Services
{
    public class TenantService : ITenantService
    {
        private readonly HttpContext _httpContext;
        private readonly TenantSettings _tenantSettings;
        private readonly Tenant? _currentTenant;

        public TenantService(IHttpContextAccessor httpAccessor, IOptions<TenantSettings> tenantOptions)
        {
            _httpContext = httpAccessor.HttpContext;
            _tenantSettings = tenantOptions.Value;
            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    _currentTenant = _tenantSettings.Tenants.FirstOrDefault(x => x.TenantId == tenantId);
                    if (_currentTenant is null)
                    {
                        throw new Exception("Invalid tenant Id");
                    }
                    _currentTenant.DbConnection ??= _tenantSettings.DefaultConfiguration.DbConnection;
                    _currentTenant.DbProvider ??= _tenantSettings.DefaultConfiguration.DbProvider;
                }
                else
                {
                    throw new Exception("No tenant Id provided");
                }
            }
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string GetDbConnection()
        {
            return _currentTenant?.DbConnection ?? _tenantSettings.DefaultConfiguration.DbConnection;
        }

        public string GetDbProvider()
        {
            return _currentTenant?.DbProvider ?? _tenantSettings.DefaultConfiguration.DbProvider;
        }
    }
}
