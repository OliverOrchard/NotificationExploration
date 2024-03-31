using Notification.Api.Configuration;
using Notification.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenForNotificationApi();
builder.Services.AddAuthenticationAndAuthorizationForNotificationApi(builder.Configuration["ApiKey"]);
builder.Services.AddAllEndpointsDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapAllEndpoints();
app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
