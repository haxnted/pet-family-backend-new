namespace VolunteerManagement.Domain.Aggregates.Volunteers.Enums;

/// <summary>
/// Статус животного.
/// </summary>
public enum HelpStatusPet
{
    /// <summary>
    /// Нуждается в помощи.
    /// </summary>
    NeedsHelp,
    
    /// <summary>
    /// Ищет новый дом.
    /// </summary>
    LookingForHome,
    
    /// <summary>
    /// Нашел дом.
    /// </summary>
    FoundHome
}