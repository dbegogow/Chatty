using Chatty.Server.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddDatabase(configuration)
    .AddIdentity()
    .AddJwtAuthentication(configuration)
    .AddApplicationServices()
    .AddRequestModelsValidators()
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddAuthorization()
    .AddAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseRouting()
    .UseEndpoints()
    .UseAuthentication()
    .UseAuthorization();

app.Run();