namespace VolunteerManagement.Tests.Integration.Fixtures;

[CollectionDefinition(Name)]
public sealed class VolunteerManagementIntegrationTestCollection
    : ICollectionFixture<VolunteerManagementWebApplicationFactory>
{
    public const string Name = "VolunteerManagementIntegration";
}
