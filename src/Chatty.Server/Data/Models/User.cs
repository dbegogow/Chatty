using Chatty.Server.Data.Models.Base;

using Microsoft.AspNetCore.Identity;

namespace Chatty.Server.Data.Models;

public class User : IdentityUser, IEntity
{
    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ModifiedBy { get; set; }
}
