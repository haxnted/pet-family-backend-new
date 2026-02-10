using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на добавление волонтера.
/// </summary>
/// <param name="FullName">Полное имя волонтёра.</param>
public sealed record AddVolunteerRequest(FullNameDto FullName);