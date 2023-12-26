namespace Multi_Tenancy.Models.Interfaces
{
    public interface IAuditableEntity
    {
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime? LastModifiedOn { get; set; }
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
