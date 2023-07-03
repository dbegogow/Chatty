using Chatty.Server.Infrastructure.Extensions;
using Chatty.Server.Infrastructure.Services;
using Chatty.Server.Mappings;
using Chatty.Server.Models.Request;
using Chatty.Server.Services.Chat;

using Microsoft.AspNetCore.Authorization;

using FluentValidation;
using Chatty.Server.Data.Models;
using Microsoft.AspNetCore.Identity;

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
                HttpContext context,
                ICurrentUserService currentUserService,
                IChatService chatService,
                IValidator<ChatsSearchRequestModel> validator) =>
            {
                var model = new ChatsSearchRequestModel
                {
                    Username = context.Request.Query[nameof(ChatsSearchRequestModel.Username)],
                    Skip = int.TryParse(
                            context.Request.Query[nameof(ChatsSearchRequestModel.Skip)],
                            out int skip)
                        ? skip : 0,
                    Take = int.TryParse(
                            context.Request.Query[nameof(ChatsSearchRequestModel.Take)],
                            out int take)
                        ? take : 0
                };

                if (model is null)
                    return Results.BadRequest();

                var validationResult = await validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var currentUserId = currentUserService.GetId();

                var chats = (await chatService
                        .Search(
                            model.Username,
                            currentUserId,
                            model.Skip,
                            model.Take))
                    .ToChatsSearchCoreModel();

                return Results.Ok(chats);
            }).RequireAuthorization();

            endpoints.MapPost("api/chat/send-message", [Authorize(Roles = "User")] async (
                HttpContext context,
                ICurrentUserService currentUserService,
                IChatService chatService,
                IValidator<MessageRequestModel> validator,
                MessageRequestModel model) =>
            {
                if (model is null)
                    return Results.BadRequest();

                var validationResult = await validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var currentUserId = currentUserService.GetId();

                var result = await chatService.SaveMessage(
                    model.Text, currentUserId, model.ReceiverUsername);

                if (result.Failure)
                {
                    return Results.BadRequest(result.Errors);
                }

                return Results.Ok();
            }).RequireAuthorization();
        });
}
