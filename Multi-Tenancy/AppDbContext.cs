using Microsoft.EntityFrameworkCore;
using Multi_Tenancy.Models.Interfaces;
using System.Linq.Expressions;

namespace Multi_Tenancy
{
    public class AppDbContext : DbContext
    {
        private readonly string _tenantId;
        private readonly string _connectionString;
        private readonly string _dbProvider;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantId = tenantService.GetCurrentTenant()?.TenantId;
            _connectionString = tenantService.GetDbConnection();
            _dbProvider = tenantService.GetDbProvider();
        }

        /// <summary>
        /// Constructor used to create instance of AppDbContext to specific tenant
        /// </summary>
        /// <param name="tenant">specified tenet (you must provide connection string and database provider name in the instance) </param>
        public AppDbContext(Tenant tenant)
        {
            _tenantId = tenant.TenantId;
            _connectionString = tenant.DbConnection;
            _dbProvider = tenant.DbProvider;
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IHaveTenant).IsAssignableFrom(entityType.ClrType))
                {
                    var param = Expression.Parameter(entityType.ClrType, "p");
                    var body = Expression.Equal(Expression.Property(param,nameof(IHaveTenant.TenantId)),Expression.Constant(_tenantId));
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body,param));
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!string.IsNullOrWhiteSpace(_connectionString))
            {
                if(_dbProvider.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(_connectionString);
                }
                else if(_dbProvider.ToLower() == "sqlite")
                {
                    optionsBuilder.UseSqlite(_connectionString);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.Id = Guid.NewGuid();
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.IsDeleted)
                        entry.Entity.DeletedOn = DateTime.UtcNow;
                    else
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedOn = DateTime.UtcNow;
                }
            }
            foreach (var entry in ChangeTracker.Entries<IHaveTenant>().Where(x => x.State == EntityState.Added))
            {
                entry.Entity.TenantId = _tenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
