using Notification.Domain.Commands.Users;
using Notification.Domain.Models;
using Notification.Domain.Queries.Shared;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Domain.Services.Users;

public class AddFriendService : IAddFriendService
{
    private readonly IStartRelationshipCommand _startRelationshipCommand;
    private readonly IEndRelationshipCommand _endRelationshipCommand;
    private readonly IGetAllActiveRelationshipsBetweenUsersQuery _getAllActiveRelationships;

    public AddFriendService(
        IStartRelationshipCommand startRelationshipCommand, 
        IEndRelationshipCommand endRelationshipCommand, 
        IGetAllActiveRelationshipsBetweenUsersQuery getAllActiveRelationships)
    {
        _startRelationshipCommand = startRelationshipCommand;
        _endRelationshipCommand = endRelationshipCommand;
        _getAllActiveRelationships = getAllActiveRelationships;
    }

    public async Task<AddFriendResult> ExecuteAsync(FriendRequest friendRequest)
    {
        var (requestingUserId, targetUserId) = friendRequest;

        var activeRelationships = await _getAllActiveRelationships.ExecuteAsync(requestingUserId, targetUserId);
        
        if (IsFriendAlreadyAdded(activeRelationships, requestingUserId, targetUserId))
        {
            return AddFriendResult.TargetUserAlreadyFriend;
        }
        
        if (IsRequestingUserBlockedByTargetUser(activeRelationships, requestingUserId, targetUserId))
        {
            return AddFriendResult.TargetUserHasBlockedRequestingUser;
        }

        if (IsTargetFriendBlocked(activeRelationships, requestingUserId, targetUserId))
        {
            await _endRelationshipCommand.ExecuteAsync(requestingUserId, targetUserId, RelationshipType.Blocked);
        }
        
        var result = await _startRelationshipCommand.ExecuteAsync(requestingUserId,targetUserId, RelationshipType.Friend);
        return result switch
        {
            StartRelationshipCommandResult.RelationshipSuccessfullyStarted => AddFriendResult.FriendSuccessfullyAdded,
            StartRelationshipCommandResult.EitherRequestingUserOrTargetUserDoesNotExist => AddFriendResult.EitherRequestingUserOrTargetUserDoesNotExist,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private bool IsFriendAlreadyAdded(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == requestingUserId
                            && relationship.TargetUserId == targetUserId
                            && relationship.RelationshipType is RelationshipType.Friend);
    }

    private bool IsRequestingUserBlockedByTargetUser(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == targetUserId
                            && relationship.TargetUserId == requestingUserId
                            && relationship.RelationshipType is RelationshipType.Blocked);
    }

    private bool IsTargetFriendBlocked(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == requestingUserId
                            && relationship.TargetUserId == targetUserId
                            && relationship.RelationshipType == RelationshipType.Blocked
        );
    }
}