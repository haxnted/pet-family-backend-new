namespace VolunteerManagement.Services.Volunteers.Dtos;

/// <summary>
/// DTO статуса процесса усыновления.
/// </summary>
public record AdoptionStatusDto(
    Guid CorrelationId,
    string CurrentState,
    Guid PetId,
    Guid VolunteerId,
    Guid AdopterId,
    string AdopterName,
    string PetNickName,
    Guid? ChatId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? FailureReason);
