using Notification.Domain.Commands.Users;
using Notification.Domain.Models;
using Notification.Domain.Queries.Shared;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Domain.Services.Users;

public class BlockUserService : IBlockUserService
{
    private readonly IGetAllActiveRelationshipsBetweenUsersQuery _getAllActiveRelationships;
    private readonly IStartRelationshipCommand _startRelationshipCommand;
    private readonly IEndRelationshipCommand _endRelationshipCommand;

    public BlockUserService(
        IStartRelationshipCommand startRelationshipCommand, 
        IEndRelationshipCommand endRelationshipCommand, 
        IGetAllActiveRelationshipsBetweenUsersQuery getAllActiveRelationships)
    {
        _startRelationshipCommand = startRelationshipCommand;
        _endRelationshipCommand = endRelationshipCommand;
        _getAllActiveRelationships = getAllActiveRelationships;
    }

    public async Task<BlockUserResult> ExecuteAsync(BlockRequest blockRequest)
    {
        var (requestingUserId, targetUserId) = blockRequest;
        var activeRelationships = await _getAllActiveRelationships.ExecuteAsync(requestingUserId, targetUserId);
        
        if (IsTargetUserAlreadyBlocked(activeRelationships, requestingUserId, targetUserId))
        {
            return BlockUserResult.TargetUserAlreadyBlocked;
        }
        
        if (IsRequestingUserBlockedByTargetUser(activeRelationships, requestingUserId, targetUserId))
        {
            return BlockUserResult.TargetUserHasBlockedRequestingUser;
        }
        
        if (IsTargetUserAFriend(activeRelationships, requestingUserId, targetUserId))
        {
            await _endRelationshipCommand.ExecuteAsync(requestingUserId, targetUserId, RelationshipType.Friend);
        }

        if (IsRequestedUserAFriend(activeRelationships, requestingUserId, targetUserId))
        {
            await _endRelationshipCommand.ExecuteAsync(targetUserId, requestingUserId, RelationshipType.Friend);
        }

        var result = await _startRelationshipCommand.ExecuteAsync(requestingUserId,targetUserId, RelationshipType.Blocked);
        return result switch
        {
            StartRelationshipCommandResult.RelationshipSuccessfullyStarted => BlockUserResult.TargetUserSuccessfullyBlocked,
            StartRelationshipCommandResult.EitherRequestingUserOrTargetUserDoesNotExist => BlockUserResult.EitherRequestingUserOrTargetUserDoesNotExist,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private bool IsTargetUserAlreadyBlocked(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == requestingUserId
                            && relationship.TargetUserId == targetUserId
                            && relationship.RelationshipType is RelationshipType.Blocked);
    }

    private bool IsRequestingUserBlockedByTargetUser(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == targetUserId
                            && relationship.TargetUserId == requestingUserId
                            && relationship.RelationshipType is RelationshipType.Blocked);
    }

    private bool IsTargetUserAFriend(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == requestingUserId
                            && relationship.TargetUserId == targetUserId
                            && relationship.RelationshipType == RelationshipType.Friend
        );
    }

    private bool IsRequestedUserAFriend(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == targetUserId
                            && relationship.TargetUserId == requestingUserId
                            && relationship.RelationshipType == RelationshipType.Friend
        );
    }
}