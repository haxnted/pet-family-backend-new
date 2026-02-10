namespace VolunteerManagement.Domain.Aggregates.Shelters.Enums;

/// <summary>
/// Статус приюта.
/// </summary>
public enum ShelterStatus
{
    /// <summary>
    /// Активный, работает.
    /// </summary>
    Active,

    /// <summary>
    /// Временно закрыт.
    /// </summary>
    TemporaryClosed,

    /// <summary>
    /// Закрыт навсегда.
    /// </summary>
    Inactive
}
