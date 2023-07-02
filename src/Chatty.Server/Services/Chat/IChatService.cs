﻿using Chatty.Server.Models.Core;

namespace Chatty.Server.Services.Chat;

public interface IChatService
{
    Task<IEnumerable<ChatCoreModel>> All(string userId);

    Task<IEnumerable<ChatsSearchCoreModel>> Search(
        string username,
        string currentUserId,
        int skip,
        int take);
}
