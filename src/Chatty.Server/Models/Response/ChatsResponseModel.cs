namespace Chatty.Server.Models.Response;

public class ChatsResponseModel
{
    public string ProfileImageUrl { get; init; }

    public IEnumerable<ChatResponseModel> Chats { get; init; }
}
