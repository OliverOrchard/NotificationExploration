namespace Notification.Domain.Models;

public enum StartRelationshipCommandResult
{
    RelationshipSuccessfullyStarted,
    EitherRequestingUserOrTargetUserDoesNotExist
}