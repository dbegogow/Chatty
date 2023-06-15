using Microsoft.AspNetCore.Identity;

namespace Chatty.Server.Data.Models;

public class User : IdentityUser
{
    public string ProfileImageUrl { get; set; }
}