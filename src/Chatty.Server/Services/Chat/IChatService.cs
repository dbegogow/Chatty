using Chatty.Server.Models.Core;

namespace Chatty.Server.Services.Chat;

public interface IChatService
{
    Task<ChatsCoreModel> All(string userId);
}
