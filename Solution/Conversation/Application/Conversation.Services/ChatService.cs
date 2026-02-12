using Conversation.Domain.Aggregates;
using Conversation.Domain.Aggregates.ValueObjects;
using Conversation.Domain.Aggregates.ValueObjects.Identifiers;
using Conversation.Domain.Aggregates.ValueObjects.Properties;
using Conversation.Services.Specifications;
using PetFamily.SharedKernel.Application.Exceptions;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Conversation.Services;

/// <summary>
/// Сервис для работы с чатами.
/// </summary>
internal sealed class ChatService(
    IRepository<Chat> repository,
    ICacheService cacheService) : IChatService
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(string title, string? description, Guid linkedId, CancellationToken ct)
    {
        var chatId = ChatId.Of(Guid.NewGuid());

        var chat = Chat.Create(
            chatId,
            Title.Of(title),
            description != null ? Description.Of(description) : null,
            linkedId);

        await repository.AddAsync(chat, ct);

        return chatId.Value;
    }

    /// <inheritdoc />
    public async Task<Guid> AddMessageAsync(
        Guid chatId, string text, Guid userId, Guid? parentMessageId, CancellationToken ct)
    {
        var chat = await GetByIdAsync(chatId, ct);

        var messageId = MessageId.Of(Guid.NewGuid());

        chat.AddMessage(
            messageId,
            MessageText.Of(text),
            userId,
            parentMessageId.HasValue ? MessageId.Of(parentMessageId.Value) : null);

        await repository.UpdateAsync(chat, ct);

        await cacheService.RemoveAsync(
            Caching.CacheKeys.ChatById(chatId), ct);

        return messageId.Value;
    }

    /// <inheritdoc />
    public async Task<Chat> GetByIdAsync(Guid chatId, CancellationToken ct)
    {
        var specification = new GetByIdWithMessagesSpecification(ChatId.Of(chatId));

        var chat = await repository.FirstOrDefaultAsync(specification, ct);

        if (chat is null)
        {
            throw new EntityNotFoundException<Chat>(chatId);
        }

        return chat;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Chat>> GetByLinkedIdAsync(Guid linkedId, CancellationToken ct)
    {
        var specification = new GetByLinkedIdSpecification(linkedId);

        return await repository.GetAll(specification, ct);
    }
}