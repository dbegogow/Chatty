namespace Chatty.Server.Models.Response;

public class ChatResponseModel
{
    public string Id { get; init; }

    public IEnumerable<ChatUserResponseModel> Users { get; init; }

    public IEnumerable<MessageResponseModel> Messages { get; init; }
}
