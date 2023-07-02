using Chatty.Server.Data;
using Chatty.Server.Models.Core;

using Microsoft.EntityFrameworkCore;

namespace Chatty.Server.Services.Chat;

public class ChatService : IChatService
{
    private readonly AppDbContext _data;

    public ChatService(AppDbContext data)
        => this._data = data;

    public async Task<IEnumerable<ChatCoreModel>> All(string userId)
    {
        var query = (from u in this._data.Users
                     join rm in this._data.Messages on u.Id equals rm.ReceiverUserId
                     join su in this._data.Users on rm.SenderUserId equals su.Id
                     join sm in this._data.Messages on u.Id equals sm.SenderUserId
                     join ru in this._data.Users on sm.ReceiverUserId equals ru.Id
                     where u.Id == userId
                     select new ChatCoreModel
                     {
                         SenderProfileImageUrl = su.ProfileImageUrl,
                         ReceiverProfileImageUrl = ru.ProfileImageUrl,
                         SenderUsername = su.UserName,
                         ReceiverUsername = ru.UserName
                     }
                    ).AsNoTracking();

        var chats = await query
            .GroupBy(c => new
            {
                c.SenderProfileImageUrl,
                c.ReceiverProfileImageUrl,
                c.SenderUsername,
                c.ReceiverUsername
            })
            .Select(g => g.First())
            .ToListAsync();

        return chats;
    }

    public async Task<IEnumerable<ChatsSearchCoreModel>> Search(
        string username,
        string currentUserId,
        int skip,
        int take)
    {
        var query = (from u in this._data.Users
                     where u.UserName.Contains(username)
                           && u.Id != currentUserId
                     orderby u.Id
                     select new ChatsSearchCoreModel
                     {
                         ProfileImageUrl = u.ProfileImageUrl,
                         Username = u.UserName
                     })
                     .Skip(skip)
                     .Take(take)
                     .AsNoTracking();

        var chats = await query.ToListAsync();

        return chats;
    }
}
