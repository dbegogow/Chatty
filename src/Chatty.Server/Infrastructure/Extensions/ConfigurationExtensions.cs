using Chatty.Server.Infrastructure.Configurations;

namespace Chatty.Server.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static DatabaseConfig GetDatabaseConfigurations(this IConfiguration configuration)
            => configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>();

    public static JwtConfig GetJwtConfigurations(this IConfiguration configuration)
            => configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>();
}
