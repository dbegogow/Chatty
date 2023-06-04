namespace Chatty.Server.Infrastructure.Services;

public interface ICurrentUserService
{
    string GetUsername();

    string GetId();
}
