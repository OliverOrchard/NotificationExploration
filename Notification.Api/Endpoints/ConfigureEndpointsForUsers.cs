using Notification.Api.Endpoints.Users;
using Notification.Data.Commands.Users;
using Notification.Data.Queries.Shared;
using Notification.Data.Queries.Users;
using Notification.Domain.Commands.Users;
using Notification.Domain.Queries.Shared;
using Notification.Domain.Queries.Users;
using Notification.Domain.Services.Users;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Api.Endpoints;

public static class ConfigureEndpointsForUsers
{
    public static void AddUsersEndpointsDependencies(this IServiceCollection services)
    {
        // Services
        services.AddScoped<ICreateUserService, CreateUserService>();
        services.AddScoped<IGetUsersService, GetUserService>();
        services.AddScoped<IAddFriendService, AddFriendService>();
        services.AddScoped<IBlockUserService, BlockUserService>();
        
        // Commands
        services.AddScoped<ICreateUserCommand, CreateUserCommand>();
        services.AddScoped<IStartRelationshipCommand, StartRelationshipCommand>();
        services.AddScoped<IEndRelationshipCommand, EndRelationshipCommand>();
        
        // Queries
        services.AddScoped<IGetUsersQuery, GetUsersQuery>();
        services.AddScoped<IGetAllActiveRelationshipsBetweenUsersQuery, GetAllActiveRelationshipsBetweenUsersQuery>();
        services.AddScoped<ICheckIfUsernameTakenQuery, CheckIfUsernameTakenQuery>();
        // services.AddScoped<ICheckIfRelationshipExistsQuery, CheckIfRelationshipExistsQuery>();
    }

    public static void MapUsersEndpoints(this IEndpointRouteBuilder builder)
    {
        var routeGroup = builder.MapGroup("/user").WithTags("User");;

        routeGroup.MapPost(string.Empty,CreateUserEndpoint.CreateUser)
            .WithName(nameof(CreateUserEndpoint.CreateUser))
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        
        routeGroup.MapPost("/block-user",BlockUserEndpoint.BlockUser)
            .WithName(nameof(BlockUserEndpoint.BlockUser))
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        
        routeGroup.MapPost("/add-friend",AddFriendEndpoint.AddFriend)
            .WithName(nameof(AddFriendEndpoint.AddFriend))
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();

        routeGroup.MapGet(string.Empty,GetUsersEndpoint.GetUsers)
            .WithName(nameof(GetUsersEndpoint.GetUsers))
            .Produces<string[]>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }
}