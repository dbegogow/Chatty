using Chatty.Server.Data.Models;
using Chatty.Server.Models.Response;
using Chatty.Server.Models.Request;
using Chatty.Server.Services.Identity;

using Microsoft.AspNetCore.Identity;

using FluentValidation;

using static Chatty.Server.Infrastructure.Constants.RolesConstants;

namespace Chatty.Server.Endpoints;

public static class AuthEndpoints
{
    public static IApplicationBuilder UseAuthEndpoints(this IApplicationBuilder builder)
        => builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("api/register", async (
                UserManager<User> userManager,
                IIdentityService identityService,
                IValidator<RegisterRequestModel> validator,
                RegisterRequestModel model) =>
            {
                if (model is null)
                    return Results.BadRequest();

                var validationResult = await validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Username
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return Results.BadRequest(result.Errors);

                await userManager.AddToRoleAsync(user, UserRole);

                var token = identityService.GenerateJwtToken(
                    user.Id,
                    user.UserName,
                    UserRole);

                return Results.Ok(new IdentityResponseModel
                {
                    Token = token
                });
            });

            endpoints.MapPost("api/login", async (
                UserManager<User> userManager,
                IIdentityService identityService,
                IValidator<LoginRequestModel> validator,
                LoginRequestModel model) =>
            {
                if (model is null)
                    return Results.BadRequest();

                var validationResult = await validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var user = await userManager.FindByEmailAsync(model.Email);

                if (user is null)
                    return Results.Unauthorized();

                var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

                if (!passwordValid)
                    return Results.Unauthorized();

                var role = (await userManager
                    .GetRolesAsync(user))
                    .FirstOrDefault();

                var token = identityService.GenerateJwtToken(
                    user.Id,
                    user.Email,
                    role);

                return Results.Ok(new IdentityResponseModel
                {
                    Token = token
                });
            });
        });
}
