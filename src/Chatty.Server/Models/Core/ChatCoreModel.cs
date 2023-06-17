namespace Chatty.Server.Models.Core;

public class ChatCoreModel
{
    public string Id { get; init; }

    public IEnumerable<ChatUserCoreModel> Users { get; init; }

    public IEnumerable<MessageCoreModel> Messages { get; init; }
}
