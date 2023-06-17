using Chatty.Server.Models.Core;
using Chatty.Server.Models.Response;

namespace Chatty.Server.Mappings;

public static class ChatMappings
{
    public static ChatsResponseModel ToChatsResponseModel(this ChatsCoreModel src)
        => new ChatsResponseModel
        {
            ProfileImageUrl = src.ProfileImageUrl,
            Chats = src.Chats.Select(c => new ChatResponseModel
            {
                Id = c.Id,
                Users = c.Users.Select(u => new ChatUserResponseModel
                {
                    ProfileImageUrl = u.ProfileImageUrl,
                    Username = u.Username
                }),
                Messages = c.Messages.Select(m => new MessageResponseModel
                {
                    Text = m.Text,
                    AuthorUsername = m.AuthorUsername
                })
            })
        };

    public static IEnumerable<ChatsSearchResponseModel> ToChatsSearchCoreModel(this IEnumerable<ChatsSearchCoreModel> src)
        => src.Select(cs => new ChatsSearchResponseModel
        {
            ProfileImageUrl = cs.ProfileImageUrl,
            Usename = cs.Usename
        })
        .ToList();
}
