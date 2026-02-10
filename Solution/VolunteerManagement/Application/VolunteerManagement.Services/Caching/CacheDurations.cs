namespace VolunteerManagement.Services.Caching;

/// <summary>
/// Константы времени жизни кеша для разных типов данных.
/// </summary>
public static class CacheDurations
{
    /// <summary>
    /// Поиск питомцев — 3 минуты.
    /// </summary>
    public static readonly TimeSpan PetSearch = TimeSpan.FromMinutes(3);

    /// <summary>
    /// Справочники (виды/породы) — 30 минут.
    /// </summary>
    public static readonly TimeSpan Species = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Приюты — 10 минут.
    /// </summary>
    public static readonly TimeSpan Shelters = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Волонтёры и питомцы — 5 минут.
    /// </summary>
    public static readonly TimeSpan Volunteers = TimeSpan.FromMinutes(5);
}
