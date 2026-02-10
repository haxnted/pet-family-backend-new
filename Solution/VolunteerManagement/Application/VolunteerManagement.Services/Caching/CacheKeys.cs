namespace VolunteerManagement.Services.Caching;

/// <summary>
/// Генерация ключей кеша для сущностей VolunteerManagement.
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// Ключ кеша для поиска питомцев.
    /// </summary>
    public static string PetSearch(
        string? nickName, Guid? speciesId, Guid? breedId, int? helpStatus,
        string? city, Guid? shelterId, DateTime? birthDateFrom, DateTime? birthDateTo,
        bool? isCastrated, bool? isVaccinated, double? weightFrom, double? weightTo,
        double? heightFrom, double? heightTo, string? sortBy, string? sortDirection,
        int page, int count)
    {
        return $"pets:search:{nickName}:{speciesId}:{breedId}:{helpStatus}:" +
               $"{city}:{shelterId}:{birthDateFrom:O}:{birthDateTo:O}:" +
               $"{isCastrated}:{isVaccinated}:{weightFrom}:{weightTo}:" +
               $"{heightFrom}:{heightTo}:{sortBy}:{sortDirection}:{page}:{count}";
    }

    /// <summary>
    /// Ключ кеша для всех видов животных.
    /// </summary>
    public static string SpeciesAll() => "species:all";

    /// <summary>
    /// Ключ кеша для вида животного по идентификатору.
    /// </summary>
    public static string SpeciesById(Guid speciesId) => $"species:{speciesId}";

    /// <summary>
    /// Ключ кеша для приюта по идентификатору.
    /// </summary>
    public static string ShelterById(Guid shelterId) => $"shelter:{shelterId}";

    /// <summary>
    /// Ключ кеша для приютов с пагинацией.
    /// </summary>
    public static string SheltersPagination(int page, int count) => $"shelters:page:{page}:{count}";

    /// <summary>
    /// Ключ кеша для волонтёра по идентификатору.
    /// </summary>
    public static string VolunteerById(Guid volunteerId) => $"volunteer:{volunteerId}";

    /// <summary>
    /// Ключ кеша для волонтёров с пагинацией.
    /// </summary>
    public static string VolunteersPagination(int page, int count) => $"volunteers:page:{page}:{count}";

    /// <summary>
    /// Ключ кеша для питомцев волонтёра.
    /// </summary>
    public static string PetsByVolunteerId(Guid volunteerId) => $"volunteer:{volunteerId}:pets";

    /// <summary>
    /// Ключ кеша для питомца по идентификатору.
    /// </summary>
    public static string PetById(Guid volunteerId, Guid petId) => $"pet:{volunteerId}:{petId}";
}