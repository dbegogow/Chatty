using System.Web;

namespace Chatty.Server.Models.Request;

public class ChatsSearchRequestModel
{
    public string Username { get; init; }

    public int Skip { get; init; }

    public int Take { get; init; }
}
