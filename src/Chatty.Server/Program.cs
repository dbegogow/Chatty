using Chatty.Server.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddDatabase(configuration)
    .AddIdentity()
    .AddJwtAuthentication(configuration)
    .AddAuthorization()
    .AddApplicationServices()
    .AddRequestModelsValidators()
    .AddEndpointsApiExplorer()
    .AddCorsPolicy()
    .AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseRouting()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints();

app.Run();