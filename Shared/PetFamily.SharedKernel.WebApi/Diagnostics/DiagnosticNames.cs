namespace PetFamily.SharedKernel.WebApi.Diagnostics;

/// <summary>
/// Централизованные названия для Activity Sources всех микросервисов
/// </summary>
public static class DiagnosticNames
{
    /// <summary>
    /// Activity Source для сервиса аутентификации
    /// </summary>
    public const string Auth = "PetFamily.Auth";

    /// <summary>
    /// Activity Source для сервиса управления волонтёрами
    /// </summary>
    public const string VolunteerManagement = "PetFamily.VolunteerManagement";

    /// <summary>
    /// Activity Source для сервиса хранения файлов
    /// </summary>
    public const string FileStorage = "PetFamily.FileStorage";

    /// <summary>
    /// Activity Source для сервиса уведомлений
    /// </summary>
    public const string Notification = "PetFamily.Notification";

    /// <summary>
    /// Activity Source для сервиса заявок волонтёров
    /// </summary>
    public const string VolunteerRequest = "PetFamily.VolunteerRequest";

    /// <summary>
    /// Activity Source для API Gateway
    /// </summary>
    public const string ApiGateway = "PetFamily.ApiGateway";
}
