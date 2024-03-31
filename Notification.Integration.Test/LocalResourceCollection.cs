using Xunit;

namespace Notification.Integration.Test;

public class LocalResourceCollection
{
    [CollectionDefinition("Local resources collection")]
    public class LocalResourcesCollection : ICollectionFixture<NotificationFixture> { }
}