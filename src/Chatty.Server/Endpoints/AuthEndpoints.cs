using Chatty.Server.Data.Models;
using Chatty.Server.Models.Request;

using Microsoft.AspNetCore.Identity;

using FluentValidation;

namespace Chatty.Server.Endpoints;

public static class AuthEndpoints
{
    public static IApplicationBuilder UseAuthEndpoints(this IApplicationBuilder builder)
        => builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("api/register", async (
                UserManager<User> userManager,
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

                return Results.Ok();
            });
        });
}
