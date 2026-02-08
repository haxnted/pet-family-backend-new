namespace PetFamily.SharedKernel.Tests.Fixtures;

[CollectionDefinition(Name)]
public sealed class PostgresContainerCollection : ICollectionFixture<PostgresContainerFixture>
{
    public const string Name = "PostgresContainer";
}