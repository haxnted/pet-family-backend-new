using PetFamily.SharedKernel.Tests.Abstractions;
using PetFamily.SharedKernel.Tests.Fakes;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Tests.Domain.Builders;

/// <summary>
/// Builder для создания тестовых экземпляров Volunteer.
/// </summary>
public sealed class VolunteerBuilder : IBuilder<Volunteer>
{
    private VolunteerId _id = VolunteerId.Of(Guid.NewGuid());

    private FullName _fullName = FullName.Of(
        FakeDataGenerator.FirstName(),
        FakeDataGenerator.LastName(),
        FakeDataGenerator.Patronymic());

    public VolunteerBuilder WithId(Guid id)
    {
        _id = VolunteerId.Of(id);
        return this;
    }

    public VolunteerBuilder WithId(VolunteerId id)
    {
        _id = id;
        return this;
    }

    public VolunteerBuilder WithFullName(string name, string surname, string? patronymic = null)
    {
        _fullName = FullName.Of(name, surname, patronymic);
        return this;
    }

    public VolunteerBuilder WithFullName(FullName fullName)
    {
        _fullName = fullName;
        return this;
    }

    public Volunteer Build()
    {
        return Volunteer.Create(_id, _fullName);
    }

    public static VolunteerBuilder Default() => new();

    public static IReadOnlyList<Volunteer> BuildMany(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => Default().Build())
            .ToList();
    }
}
