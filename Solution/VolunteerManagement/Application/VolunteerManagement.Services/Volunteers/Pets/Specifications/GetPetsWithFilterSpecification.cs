using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;

namespace VolunteerManagement.Services.Volunteers.Pets.Specifications;

/// <summary>
/// Спецификация для поиска животных по фильтрам с сортировкой и пагинацией.
/// </summary>
public sealed class GetPetsWithFilterSpecification : Specification<Pet>
{
    /// <summary>
    /// Создаёт спецификацию для поиска животных.
    /// </summary>
    /// <param name="nickName">Кличка (частичное совпадение).</param>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="helpStatus">Статус помощи.</param>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="birthDateFrom">Дата рождения от.</param>
    /// <param name="birthDateTo">Дата рождения до.</param>
    /// <param name="isCastrated">Кастрирован.</param>
    /// <param name="isVaccinated">Вакцинирован.</param>
    /// <param name="weightFrom">Вес от.</param>
    /// <param name="weightTo">Вес до.</param>
    /// <param name="heightFrom">Рост от.</param>
    /// <param name="heightTo">Рост до.</param>
    /// <param name="sortBy">Поле сортировки.</param>
    /// <param name="sortDirection">Направление сортировки (asc/desc).</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="count">Количество элементов на странице.</param>
    public GetPetsWithFilterSpecification(
        string? nickName,
        Guid? speciesId,
        Guid? breedId,
        int? helpStatus,
        Guid? shelterId,
        DateTime? birthDateFrom,
        DateTime? birthDateTo,
        bool? isCastrated,
        bool? isVaccinated,
        double? weightFrom,
        double? weightTo,
        double? heightFrom,
        double? heightTo,
        string? sortBy,
        string? sortDirection,
        int page,
        int count)
    {
        Query.Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nickName))
            Query.Where(p => p.NickName.Value.Contains(nickName));

        if (speciesId.HasValue)
            Query.Where(p => p.SpeciesId == speciesId.Value);

        if (breedId.HasValue)
            Query.Where(p => p.BreedId == breedId.Value);

        if (helpStatus.HasValue)
            Query.Where(p => p.HelpStatus == (HelpStatusPet)helpStatus.Value);

        if (shelterId.HasValue)
            Query.Where(p => p.ShelterId == shelterId.Value);

        if (birthDateFrom.HasValue)
            Query.Where(p => p.BirthDate >= birthDateFrom.Value);

        if (birthDateTo.HasValue)
            Query.Where(p => p.BirthDate <= birthDateTo.Value);

        if (isCastrated.HasValue)
            Query.Where(p => p.IsCastrated == isCastrated.Value);

        if (isVaccinated.HasValue)
            Query.Where(p => p.IsVaccinated == isVaccinated.Value);

        if (weightFrom.HasValue)
            Query.Where(p => p.PhysicalAttributes.Weight >= weightFrom.Value);

        if (weightTo.HasValue)
            Query.Where(p => p.PhysicalAttributes.Weight <= weightTo.Value);

        if (heightFrom.HasValue)
            Query.Where(p => p.PhysicalAttributes.Height >= heightFrom.Value);

        if (heightTo.HasValue)
            Query.Where(p => p.PhysicalAttributes.Height <= heightTo.Value);

        var isDescending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        switch (sortBy?.ToLowerInvariant())
        {
            case "nickname":
                if (isDescending)
                    Query.OrderByDescending(p => p.NickName.Value);
                else
                    Query.OrderBy(p => p.NickName.Value);
                break;
            case "birthdate":
                if (isDescending)
                    Query.OrderByDescending(p => p.BirthDate);
                else
                    Query.OrderBy(p => p.BirthDate);
                break;
            case "weight":
                if (isDescending)
                    Query.OrderByDescending(p => p.PhysicalAttributes.Weight);
                else
                    Query.OrderBy(p => p.PhysicalAttributes.Weight);
                break;
            case "height":
                if (isDescending)
                    Query.OrderByDescending(p => p.PhysicalAttributes.Height);
                else
                    Query.OrderBy(p => p.PhysicalAttributes.Height);
                break;
            default:
                if (isDescending)
                    Query.OrderByDescending(p => p.DateCreated);
                else
                    Query.OrderBy(p => p.DateCreated);
                break;
        }

        Query.Skip((page - 1) * count).Take(count);
    }
}