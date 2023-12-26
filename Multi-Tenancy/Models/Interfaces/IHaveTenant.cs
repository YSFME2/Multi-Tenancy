using System.ComponentModel.DataAnnotations;

namespace Multi_Tenancy.Models.Interfaces
{
    public interface IHaveTenant
    {
        string TenantId { get; set; }
    }
}
