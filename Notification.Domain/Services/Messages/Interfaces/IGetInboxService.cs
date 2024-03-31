using Notification.Domain.Models;

namespace Notification.Domain.Services.Messages.Interfaces;

public interface IGetInboxService
{
    Task<Message[]> ExecuteAsync(int userId);
}