using Microsoft.AspNetCore.Identity;

namespace Chatty.Server.Data.Models;

public class User : IdentityUser
{
    public string ProfileImageUrl { get; set; }

    public ICollection<Message> SendedMessages { get; init; } = new HashSet<Message>();

    public ICollection<Message> ReceivedMessages { get; init; } = new HashSet<Message>();
}