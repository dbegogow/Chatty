using Chatty.Server.Data;

using Microsoft.EntityFrameworkCore;

namespace Chatty.Server.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDbContext<AppDbContext>(options => options
                    .UseSqlServer(configuration.GetDefaultConnectionString()));
}
