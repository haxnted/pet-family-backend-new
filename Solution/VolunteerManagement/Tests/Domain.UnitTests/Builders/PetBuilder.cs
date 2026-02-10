using PetFamily.SharedKernel.Tests.Abstractions;
using PetFamily.SharedKernel.Tests.Fakes;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Tests.Domain.Builders;

/// <summary>
/// Builder для создания тестовых экземпляров Pet.
/// </summary>
public sealed class PetBuilder : IBuilder<Pet>
{
    private PetId _id = PetId.Of(Guid.NewGuid());
    private VolunteerId _volunteerId = VolunteerId.Of(Guid.NewGuid());
    private NickName _nickName = NickName.Of("ТестовыйПитомец"); // 10–500 символов, только буквы и дефис
    private Description _generalDescription = Description.Of("Достаточно длинное описание для теста питомца не менее десяти символов.");
    private Description _healthInformation = Description.Of("Информация о здоровье питомца для тестовых сценариев.");
    private PetPhysicalAttributes _attributes = PetPhysicalAttributes.Of(25.5, 50.0);
    private Guid _speciesId = Guid.NewGuid();
    private Guid _breedId = Guid.NewGuid();
    private DateTime _birthDate = FakeDataGenerator.PastDate(3);
    private bool _isCastrated = FakeDataGenerator.Bool();
    private bool _isVaccinated = FakeDataGenerator.Bool();
    private HelpStatusPet _helpStatus = HelpStatusPet.NeedsHelp;
    private List<Requisite> _requisites = [];

    public PetBuilder WithId(Guid id)
    {
        _id = PetId.Of(id);
        return this;
    }

    public PetBuilder WithVolunteerId(Guid volunteerId)
    {
        _volunteerId = VolunteerId.Of(volunteerId);
        return this;
    }

    public PetBuilder WithVolunteerId(VolunteerId volunteerId)
    {
        _volunteerId = volunteerId;
        return this;
    }

    public PetBuilder WithNickName(string nickName)
    {
        _nickName = NickName.Of(nickName);
        return this;
    }

    public PetBuilder WithGeneralDescription(string description)
    {
        _generalDescription = Description.Of(description);
        return this;
    }

    public PetBuilder WithHealthInformation(string healthInfo)
    {
        _healthInformation = Description.Of(healthInfo);
        return this;
    }

    public PetBuilder WithPhysicalAttributes(double weight, double height)
    {
        _attributes = PetPhysicalAttributes.Of(weight, height);
        return this;
    }

    public PetBuilder WithSpeciesId(Guid speciesId)
    {
        _speciesId = speciesId;
        return this;
    }

    public PetBuilder WithBreedId(Guid breedId)
    {
        _breedId = breedId;
        return this;
    }

    public PetBuilder WithBirthDate(DateTime birthDate)
    {
        _birthDate = birthDate;
        return this;
    }

    public PetBuilder Castrated(bool isCastrated = true)
    {
        _isCastrated = isCastrated;
        return this;
    }

    public PetBuilder Vaccinated(bool isVaccinated = true)
    {
        _isVaccinated = isVaccinated;
        return this;
    }

    public PetBuilder WithHelpStatus(HelpStatusPet status)
    {
        _helpStatus = status;
        return this;
    }

    public PetBuilder WithRequisites(params Requisite[] requisites)
    {
        _requisites = requisites.ToList();
        return this;
    }

    public Pet Build()
    {
        return Pet.Create(
            _id,
            _volunteerId,
            _nickName,
            _generalDescription,
            _healthInformation,
            _attributes,
            _speciesId,
            _breedId,
            _birthDate,
            _isCastrated,
            _isVaccinated,
            _helpStatus,
            _requisites);
    }

    public static PetBuilder Default() => new();

    public static IReadOnlyList<Pet> BuildMany(int count, VolunteerId? volunteerId = null)
    {
        return Enumerable.Range(0, count)
            .Select(_ =>
            {
                var builder = Default();
                if (volunteerId != null)
                    builder.WithVolunteerId(volunteerId.Value);
                return builder.Build();
            })
            .ToList();
    }
}
