namespace PetFamily.SharedKernel.Tests.Fixtures;

[CollectionDefinition(Name)]
public sealed class RabbitMqContainerCollection : ICollectionFixture<RabbitMqContainerFixture>
{
    public const string Name = "RabbitMqContainer";
}