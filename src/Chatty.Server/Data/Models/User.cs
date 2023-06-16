using Microsoft.AspNetCore.Identity;

namespace Chatty.Server.Data.Models;

public class User : IdentityUser
{
    public string ProfileImageUrl { get; set; }

    public ICollection<Chat> Chats { get; init; } = new HashSet<Chat>();

    public ICollection<Message> Messages { get; init; } = new HashSet<Message>();
}