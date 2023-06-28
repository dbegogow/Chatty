using Chatty.Server.Infrastructure.Extensions;
using Chatty.Server.Infrastructure.Services;
using Chatty.Server.Mappings;
using Chatty.Server.Models.Request;
using Chatty.Server.Services.Chat;

using Microsoft.AspNetCore.Authorization;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chatty.Server.Endpoints;

public static class ChatEndpoints
{
    public static IApplicationBuilder UseChatEndpoints(this IApplicationBuilder builder)
        => builder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("api/chats", [Authorize(Roles = "User")] async (
                ICurrentUserService currentUserService,
                IChatService chatService) =>
            {
                var userId = currentUserService.GetId();

                var chats = (await chatService.All(userId)).ToChatsResponseModel();

                return Results.Ok(chats);
            }).RequireAuthorization();

            endpoints.MapGet("api/chats/search", [Authorize(Roles = "User")] async (
                IChatService chatService,
                IValidator<ChatsSearchRequestModel> validator,
                [FromQuery] ChatsSearchRequestModel model) =>
            {
                if (model is null)
                    return Results.BadRequest();

                var validationResult = await validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var chats = (await chatService.Search(model.Username)).ToChatsSearchCoreModel();

                return Results.Ok(chats);
            }).RequireAuthorization();
        });
}
