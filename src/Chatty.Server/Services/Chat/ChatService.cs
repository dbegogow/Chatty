﻿using Chatty.Server.Data;
using Chatty.Server.Data.Models;
using Chatty.Server.Models;
using Chatty.Server.Models.Core;

using Microsoft.EntityFrameworkCore;

using static Chatty.Server.Infrastructure.Constants.ErrorMessagesConstants;

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

    public async Task<Result<bool>> SaveMessage(string text, string senderUserId, string receiverUserUsername)
    {
        var result = new Result<bool>();

        var receiverUser = await this.GetDbUserByUsername(receiverUserUsername);

        if (receiverUser == null)
        {
            result.Errors.Add(NotExistReceiverUser);
            return result;
        }

        var newMessage = new Message
        {
            Text = text,
            SenderUserId = senderUserId,
            ReceiverUser = receiverUser
        };

        this._data.Messages.Add(newMessage);

        await this._data.SaveChangesAsync();

        return result;
    }

    private async Task<User> GetDbUserByUsername(string username)
        => await (from u in this._data.Users
                  where u.UserName == username
                  select u
                 ).FirstOrDefaultAsync();
}
