using Notification.Domain.Models;

namespace Notification.Domain.Services.Users.Interfaces;

public interface IBlockUserService
{
    Task<BlockUserResult> ExecuteAsync(BlockRequest friendRequest);
}