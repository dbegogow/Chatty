namespace Chatty.Server.Data.Models.Base;

public class DeletableEntity : IDeletableEntity
{
    public DateTime? DeletedOn { get; set; }

    public string DeletedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ModifiedBy { get; set; }
}
