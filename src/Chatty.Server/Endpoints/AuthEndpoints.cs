using Chatty.Server.Models.Request;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Chatty.Server.Endpoints;

public static class AuthEndpoints
{
    public static IApplicationBuilder UseAuthEndpoints(this IApplicationBuilder builder)
        => builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("api/register", async (
                IValidator<RegisterRequestModel> validator,
                RegisterRequestModel model) =>
            {
                if (model is null)
                    return Results.BadRequest();

                var validationResult = await validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());


                return Results.Ok();
            });
        });
}
