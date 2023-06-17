using Chatty.Server.Data;
using Chatty.Server.Models.Core;

using Microsoft.EntityFrameworkCore;

namespace Chatty.Server.Services.Chat;

public class ChatService : IChatService
{
    private readonly AppDbContext _data;

    public ChatService(AppDbContext data)
        => this._data = data;

    public async Task<ChatsCoreModel> All(string userId)
    {
        var query = (from u in this._data.Users
                     where u.Id == userId
                     select new ChatsCoreModel
                     {
                         ProfileImageUrl = u.ProfileImageUrl,
                         Chats = u.Chats.Select(c => new ChatCoreModel
                         {
                             Id = c.Id,
                             Users = c.Users
                                .Where(cu => u.Id != userId)
                                .Select(cu => new ChatUserCoreModel
                                {
                                    ProfileImageUrl = cu.ProfileImageUrl,
                                    Username = cu.UserName
                                }),
                             Messages = c.Messages
                                .OrderByDescending(m => m.CreatedOn)
                                .Select(m => new MessageCoreModel
                                {
                                    Text = m.Text,
                                    AuthorUsername = m.User.UserName
                                })
                         })
                     }).AsNoTracking();

        var chat = await query.FirstOrDefaultAsync();

        return chat;
    }

    public async Task<IEnumerable<ChatsSearchCoreModel>> Search(string username)
    {
        var query = (from u in this._data.Users
                     where u.UserName.Contains(username)
                     select new ChatsSearchCoreModel
                     {
                         ProfileImageUrl = u.ProfileImageUrl,
                         Usename = u.UserName
                     }).AsNoTracking();

        var chats = await query.ToListAsync();

        return chats;
    }
}
