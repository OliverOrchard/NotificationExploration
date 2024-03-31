using Notification.Api.Endpoints.Messages;
using Notification.Data.Commands.Messages;
using Notification.Data.Queries.Messages;
using Notification.Domain.Commands.Messages;
using Notification.Domain.Queries.Messages;
using Notification.Domain.Services.Messages;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Api.Endpoints;

public static class ConfigureEndpointsForMessages
{
    public static void AddMessagesEndpointsDependencies(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IGetOutboxService,GetOutboxService>();
        services.AddScoped<IGetInboxService,GetInboxService>();
        services.AddScoped<ICreateMessageService,CreateMessageService>();

        // Commands
        services.AddScoped<ICreateMessageCommand,CreateMessageCommand>();
        
        // Queries
        services.AddScoped<IGetAllBlockRelationshipUserIds,GetAllBlockRelationshipUserIds>();
        services.AddScoped<IGetInboxForUserQuery,GetInboxForUserQuery>();
        services.AddScoped<IGetOutboxForUserQuery,GetOutboxForUserQuery>();
    }

    public static void MapMessagesEndpoints(this IEndpointRouteBuilder builder)
    {
        var routeGroup = builder.MapGroup("/user/{userId}/message").WithTags("Message");;

        routeGroup.MapPost(string.Empty, CreateMessageEndpoint.CreateMessage)
            .WithName(nameof(CreateMessageEndpoint.CreateMessage))
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        routeGroup.MapGet("/inbox",GetInboxEndpoint.GetInbox)
            .WithName(nameof(GetInboxEndpoint.GetInbox))
            .Produces<string[]>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
        
        routeGroup.MapGet("/outbox",GetOutboxEndpoint.GetOutbox)
            .WithName(nameof(GetOutboxEndpoint.GetOutbox))
            .Produces<string[]>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }
}