using Notification.Domain.Models;

namespace Notification.Domain.Services.Users.Interfaces;

public interface IAddFriendService
{
    Task<AddFriendResult> ExecuteAsync(FriendRequest friendRequest);
}