using Chatty.Server.Data.Models.Base;

namespace Chatty.Server.Data.Models;

public class Chat : DeletableEntity
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public ICollection<Message> Messages { get; init; } = new HashSet<Message>();

    public ICollection<User> Users { get; init; } = new HashSet<User>();
}
