using Notification.Domain.Commands.Messages;
using Notification.Domain.Models;
using Notification.Domain.Queries.Shared;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Domain.Services.Messages;

public class CreateMessageService : ICreateMessageService
{
    private readonly IGetAllActiveRelationshipsBetweenUsersQuery _getAllActiveRelationships;
    private readonly ICreateMessageCommand _createMessageCommand;

    public CreateMessageService(
        IGetAllActiveRelationshipsBetweenUsersQuery getAllActiveRelationships, 
        ICreateMessageCommand createMessageCommand)
    {
        _getAllActiveRelationships = getAllActiveRelationships;
        _createMessageCommand = createMessageCommand;
    }

    public async Task<MessageCreatedResult> ExecuteAsync(int senderId, int messageRequestRecipientId, string messageRequestMessage)
    {
        var activeRelationships = await _getAllActiveRelationships.ExecuteAsync(senderId, messageRequestRecipientId);

        if (IsSenderBlockedByRecipient(activeRelationships,senderId,messageRequestRecipientId))
        {
            return MessageCreatedResult.RecipientHasBlockedRequestingUser;
        }

        if (IsRecipientBlocked(activeRelationships,senderId,messageRequestRecipientId))
        {
            return MessageCreatedResult.RecipientIsBlocked;
        }

        if (IsRecipientNotAFriend(activeRelationships,senderId,messageRequestRecipientId))
        {
            return MessageCreatedResult.RecipientIsNotAFriend;
        }
        
        var result = await _createMessageCommand.ExecuteAsync(senderId,messageRequestRecipientId,messageRequestMessage);;
        return result switch
        {
            CreateMessageCommandResult.MessageSuccessfullyCreated => MessageCreatedResult.MessageCreatedSuccessfully,
            CreateMessageCommandResult.EitherSenderOrRecipientDoesNotExist => MessageCreatedResult.EitherSenderOrRecipientDoesNotExist,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private bool IsSenderBlockedByRecipient(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == targetUserId
                            && relationship.TargetUserId == requestingUserId
                            && relationship.RelationshipType is RelationshipType.Blocked);
    }

    private bool IsRecipientBlocked(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return activeRelationships.Any(
            relationship => relationship.RequestingUserId == requestingUserId
                            && relationship.TargetUserId == targetUserId
                            && relationship.RelationshipType is RelationshipType.Blocked);
    }

    private bool IsRecipientNotAFriend(UserRelationship[] activeRelationships,int requestingUserId, int targetUserId)
    {
        return !activeRelationships.Any(
            relationship => relationship.RequestingUserId == requestingUserId
                            && relationship.TargetUserId == targetUserId
                            && relationship.RelationshipType is RelationshipType.Friend);
    }
}