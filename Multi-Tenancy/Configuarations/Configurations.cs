using Microsoft.EntityFrameworkCore;
using Multi_Tenancy.Services;

namespace Multi_Tenancy.Configurations
{
    public static class Configurations
    {
        public static WebApplicationBuilder AddTenancy(this WebApplicationBuilder builder)
        {
            var tenantSettingsSection = builder.Configuration.GetSection(nameof(TenantSettings));
            builder.Services.Configure<TenantSettings>(tenantSettingsSection);
            var tenantSettings = new TenantSettings();
            tenantSettingsSection.Bind(tenantSettings);
            builder.Services.AddScoped<ITenantService, TenantService>();
            foreach (var tenant in tenantSettings.Tenants)
            {
                var connectionString = tenant.DbConnection ?? tenantSettings.DefaultConfiguration.DbConnection;
                tenant.DbConnection ??= tenantSettings.DefaultConfiguration.DbConnection;
                tenant.DbProvider ??= tenantSettings.DefaultConfiguration.DbProvider;

                using (var dbContext = new AppDbContext(tenant))
                {
                    if (dbContext.Database.GetPendingMigrations().Any())
                        dbContext.Database.Migrate();
                }
            }

            return builder;
        }
    }
}
