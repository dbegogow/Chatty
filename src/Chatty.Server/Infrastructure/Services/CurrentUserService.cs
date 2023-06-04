using System.Security.Claims;

using Chatty.Server.Infrastructure.Extensions;

namespace Chatty.Server.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal user;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
           => user = httpContextAccessor.HttpContext?.User;

    public string GetUsername()
             => user?.Identity?.Name;

    public string GetId()
        => user?.GetId();
}
