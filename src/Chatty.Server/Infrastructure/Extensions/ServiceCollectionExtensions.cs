using System.Text;

using Chatty.Server.Data;
using Chatty.Server.Data.Models;
using Chatty.Server.Infrastructure.Services;
using Chatty.Server.Models.Request;
using Chatty.Server.Models.Request.Validators;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using FluentValidation;

namespace Chatty.Server.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<AppDbContext>(options => options
                .UseSqlServer(configuration.GetDatabaseConfigurations().DefaultConnection));

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetJwtConfigurations();

        var key = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);

        service
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return service;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services
            .AddScoped<ICurrentUserService, CurrentUserService>();

    public static IServiceCollection AddRequestModelsValidators(this IServiceCollection services)
        => services
            .AddScoped<IValidator<RegisterRequestModel>, RegisterRequestModelValidator>();

    public static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Chatty Server",
                    Version = "v1"
                });

            c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
}
