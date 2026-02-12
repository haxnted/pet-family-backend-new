using Conversation.Handlers.Commands.CreateChat;
using Conversation.Handlers.Commands.SendMessage;
using Conversation.Handlers.Queries.GetChatById;
using Conversation.Handlers.Queries.GetChatsByLinkedId;
using Conversation.Hosts.Endpoints.Requests;
using Conversation.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.SharedKernel.WebApi.Services;
using Wolverine;

namespace Conversation.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для работы с чатами и сообщениями.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
/// <param name="currentUser">Сервис для получения информации о текущем пользователе.</param>
[ApiController]
[Route("api/chats")]
public class ChatController(
    IMessageBus bus,
    ICurrentUser currentUser) : ControllerBase
{
    /// <summary>
    /// Создать чат.
    /// </summary>
    /// <param name="request">Запрос на создание чата.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateChat(
        [FromBody] CreateChatRequest request,
        CancellationToken ct)
    {
        var command = new CreateChatCommand
        {
            Title = request.Title,
            Description = request.Description,
            LinkedId = request.LinkedId,
        };

        var chatId = await bus.InvokeAsync<Guid>(command, ct);

        return Ok(chatId);
    }

    /// <summary>
    /// Отправить сообщение в чат.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="request">Запрос на отправку сообщения.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("{chatId:guid}/messages")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(
        [FromRoute] Guid chatId,
        [FromBody] SendMessageRequest request,
        CancellationToken ct)
    {
        var command = new SendMessageCommand
        {
            ChatId = chatId,
            Text = request.Text,
            UserId = currentUser.UserId,
            ParentMessageId = request.ParentMessageId,
        };

        var messageId = await bus.InvokeAsync<Guid>(command, ct);

        return Ok(messageId);
    }

    /// <summary>
    /// Получить чат по идентификатору (с сообщениями).
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{chatId:guid}")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(typeof(ChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChatById(
        [FromRoute] Guid chatId,
        CancellationToken ct)
    {
        var query = new GetChatByIdQuery(chatId);
        var result = await bus.InvokeAsync<ChatDto>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Получить чаты по идентификатору связанной сущности.
    /// </summary>
    /// <param name="linkedId">Идентификатор связанной сущности.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("linked/{linkedId:guid}")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(typeof(List<ChatDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChatsByLinkedId(
        [FromRoute] Guid linkedId,
        CancellationToken ct)
    {
        var query = new GetChatsByLinkedIdQuery(linkedId);
        var result = await bus.InvokeAsync<List<ChatDto>>(query, ct);

        return Ok(result);
    }
}
