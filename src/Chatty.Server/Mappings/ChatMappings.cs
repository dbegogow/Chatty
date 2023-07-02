using Chatty.Server.Models.Core;
using Chatty.Server.Models.Response;

namespace Chatty.Server.Mappings;

public static class ChatMappings
{
    public static IEnumerable<ChatsSearchResponseModel> ToChatsSearchCoreModel(this IEnumerable<ChatsSearchCoreModel> src)
        => src.Select(cs => new ChatsSearchResponseModel
        {
            ProfileImageUrl = cs.ProfileImageUrl,
            Username = cs.Username
        })
        .ToList();
}
