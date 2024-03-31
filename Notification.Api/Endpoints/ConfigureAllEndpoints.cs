using Notification.Data;

namespace Notification.Api.Endpoints;

public static class ConfigureAllEndpoints
{
    public static void AddAllEndpointsDependencies(this IServiceCollection services)
    {
        services.AddSharedDependencies();
        
        services.AddUsersEndpointsDependencies();
        services.AddMessagesEndpointsDependencies();
    }

    public static void MapAllEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapUsersEndpoints();
        builder.MapMessagesEndpoints();
    }

    private static void AddSharedDependencies(this IServiceCollection services)
    {
        services.AddScoped<IStorageProvider, StorageProvider>();
    }
}