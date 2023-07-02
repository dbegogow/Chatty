using Chatty.Server.Data;
using Chatty.Server.Models.Core;

using Microsoft.EntityFrameworkCore;

namespace Chatty.Server.Services.Chat;

public class ChatService : IChatService
{
    private readonly AppDbContext _data;

    public ChatService(AppDbContext data)
        => this._data = data;

    public async Task<bool> All(string userId)
    {
        return await Task.FromResult(true);
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
