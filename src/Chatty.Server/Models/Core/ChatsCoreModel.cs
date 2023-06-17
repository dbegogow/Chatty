namespace Chatty.Server.Models.Core;

public class ChatsCoreModel
{
    public string ProfileImageUrl { get; init; }

    public IEnumerable<ChatCoreModel> Chats { get; init; }
}
