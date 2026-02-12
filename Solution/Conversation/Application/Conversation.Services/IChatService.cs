using Conversation.Domain.Aggregates;

namespace Conversation.Services;

/// <summary>
/// Интерфейс сервиса для работы с чатами.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Создать чат.
    /// </summary>
    /// <param name="title">Название чата.</param>
    /// <param name="description">Описание чата.</param>
    /// <param name="linkedId">Идентификатор связанной сущности.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<Guid> CreateAsync(string title, string? description, Guid linkedId, CancellationToken ct);

    /// <summary>
    /// Добавить сообщение в чат.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="text">Текст сообщения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="parentMessageId">Идентификатор родительского сообщения.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<Guid> AddMessageAsync(Guid chatId, string text, Guid userId, Guid? parentMessageId, CancellationToken ct);

    /// <summary>
    /// Получить чат по идентификатору (с сообщениями).
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<Chat> GetByIdAsync(Guid chatId, CancellationToken ct);

    /// <summary>
    /// Получить чаты по идентификатору связанной сущности.
    /// </summary>
    /// <param name="linkedId">Идентификатор сущности подвязанной к чату.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<IReadOnlyList<Chat>> GetByLinkedIdAsync(Guid linkedId, CancellationToken ct);
}