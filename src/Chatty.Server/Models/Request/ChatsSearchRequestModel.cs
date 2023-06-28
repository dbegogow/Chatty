using System.Web;

namespace Chatty.Server.Models.Request;

public class ChatsSearchRequestModel
{
    public string Username { get; init; }

    public int Skip { get; init; }

    public int Take { get; init; }

    public static bool TryParse(string queryString, out ChatsSearchRequestModel result)
    {
        var parsedValues = HttpUtility.ParseQueryString(queryString);

        result = new ChatsSearchRequestModel
        {
            Username = parsedValues["username"],
            Skip = int.Parse(parsedValues["skip"]),
            Take = int.Parse(parsedValues["take"])
        };

        return true;
    }
}
