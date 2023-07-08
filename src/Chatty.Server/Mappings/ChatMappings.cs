using Chatty.Server.Models.Core;
using Chatty.Server.Models.Response;

namespace Chatty.Server.Mappings;

public static class ChatMappings
{
    public static IEnumerable<ChatResponseModel> ToChatsResponseModel(this IEnumerable<ChatCoreModel> src)
        => src.Select(c => new ChatResponseModel
        {
            Username = c.Username,
            ProfileImageUrl = c.ProfileImageUrl
        })
        .ToList();

    public static IEnumerable<ChatsSearchResponseModel> ToChatsSearchCoreModel(this IEnumerable<ChatsSearchCoreModel> src)
        => src.Select(cs => new ChatsSearchResponseModel
        {
            ProfileImageUrl = cs.ProfileImageUrl,
            Username = cs.Username
        })
        .ToList();
}
