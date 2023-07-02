using Chatty.Server.Models.Core;
using Chatty.Server.Models.Response;

namespace Chatty.Server.Mappings;

public static class ChatMappings
{
    public static IEnumerable<ChatResponseModel> ToChatsResponseModel(this IEnumerable<ChatCoreModel> src)
        => src.Select(c => new ChatResponseModel
        {
            ProfileImageUrl = c.SenderProfileImageUrl,
            Username = c.SenderUsername
        }).Concat(src.Select(c => new ChatResponseModel
        {
            ProfileImageUrl = c.ReceiverProfileImageUrl,
            Username = c.ReceiverUsername
        }));

    public static IEnumerable<ChatsSearchResponseModel> ToChatsSearchCoreModel(this IEnumerable<ChatsSearchCoreModel> src)
        => src.Select(cs => new ChatsSearchResponseModel
        {
            ProfileImageUrl = cs.ProfileImageUrl,
            Username = cs.Username
        })
        .ToList();
}
