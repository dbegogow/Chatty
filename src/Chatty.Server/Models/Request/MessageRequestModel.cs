namespace Chatty.Server.Models.Request;

public class MessageRequestModel
{
    public string Text { get; init; }

    public string ReceiverUsername { get; init; }
}
