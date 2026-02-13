using PetFamily.SharedKernel.Tests.Abstractions;
using PetFamily.SharedKernel.Tests.Fakes;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Tests.Domain.Builders;

/// <summary>
/// Builder для создания тестовых экземпляров Shelter.
/// </summary>
public sealed class ShelterBuilder : IBuilder<Shelter>
{
    private ShelterId _id = ShelterId.Of(Guid.NewGuid());
    private ShelterName _name = ShelterName.Of("Тестовый приют");
    private Address _address = Address.Of("ул. Тестовая 1", "Москва", "Московская область", "123456");
    private PhoneNumber _phoneNumber = PhoneNumber.Of("71234567890");
    private Description _description = Description.Of("Описание тестового приюта для животных в городе Москва");
    private WorkingHours _workingHours = WorkingHours.Of(new TimeOnly(9, 0), new TimeOnly(18, 0));
    private int _capacity = 50;

    public ShelterBuilder WithId(Guid id)
    {
        _id = ShelterId.Of(id);
        return this;
    }

    public ShelterBuilder WithName(string name)
    {
        _name = ShelterName.Of(name);
        return this;
    }

    public ShelterBuilder WithAddress(string street, string city, string state, string zipCode)
    {
        _address = Address.Of(street, city, state, zipCode);
        return this;
    }

    public ShelterBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = PhoneNumber.Of(phoneNumber);
        return this;
    }

    public ShelterBuilder WithDescription(string description)
    {
        _description = Description.Of(description);
        return this;
    }

    public ShelterBuilder WithWorkingHours(TimeOnly open, TimeOnly close)
    {
        _workingHours = WorkingHours.Of(open, close);
        return this;
    }

    public ShelterBuilder WithCapacity(int capacity)
    {
        _capacity = capacity;
        return this;
    }

    public Shelter Build()
    {
        return Shelter.Create(_id, _name, _address, _phoneNumber, _description, _workingHours, _capacity);
    }

    public static ShelterBuilder Default() => new();

    public static IReadOnlyList<Shelter> BuildMany(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => Default().Build())
            .ToList();
    }
}
