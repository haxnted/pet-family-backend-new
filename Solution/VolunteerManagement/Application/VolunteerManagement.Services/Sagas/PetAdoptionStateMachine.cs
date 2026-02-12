using MassTransit;
using Notification.Contracts.Events;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using VolunteerManagement.Infrastructure.SagaStates;

namespace VolunteerManagement.Services.Sagas;

/// <summary>
/// Машина состояний саги усыновления питомца.
/// <remarks>
/// Оркестрирует процесс усыновления через шаги:
/// Initially → Reserving → CreatingChat → WaitingForAdoption → Adopting → Final.
/// При отклонении волонтёром: WaitingForAdoption → Compensating → Final.
/// При сбое создания чата: CreatingChat → Compensating → Final.
/// </remarks>
/// </summary>
public class PetAdoptionStateMachine : MassTransitStateMachine<PetAdoptionState>
{
    /// <summary>
    /// Состояние: питомец бронируется.
    /// </summary>
    public State Reserving { get; private set; } = null!;

    /// <summary>
    /// Состояние: создаётся чат усыновления.
    /// </summary>
    public State CreatingChat { get; private set; } = null!;

    /// <summary>
    /// Состояние: ожидание подтверждения усыновления волонтёром.
    /// </summary>
    public State WaitingForAdoption { get; private set; } = null!;

    /// <summary>
    /// Состояние: выполняется усыновление (обновление статуса питомца).
    /// </summary>
    public State Adopting { get; private set; } = null!;

    /// <summary>
    /// Состояние: выполняется компенсация (отмена бронирования).
    /// </summary>
    public State Compensating { get; private set; } = null!;

    /// <summary>
    /// Событие: инициация саги усыновления.
    /// </summary>
    public Event<StartPetAdoption> StartPetAdoptionEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: питомец успешно забронирован.
    /// </summary>
    public Event<PetReserved> PetReservedEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: бронирование питомца не удалось.
    /// </summary>
    public Event<PetReservationFailed> PetReservationFailedEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: чат усыновления успешно создан.
    /// </summary>
    public Event<AdoptionChatCreated> AdoptionChatCreatedEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: не удалось создать чат усыновления.
    /// </summary>
    public Event<AdoptionChatCreationFailed> AdoptionChatCreationFailedEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: волонтёр подтвердил усыновление.
    /// </summary>
    public Event<ConfirmAdoption> ConfirmAdoptionEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: волонтёр отклонил усыновление.
    /// </summary>
    public Event<RejectAdoption> RejectAdoptionEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: питомец успешно усыновлён.
    /// </summary>
    public Event<PetAdopted> PetAdoptedEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: не удалось обновить статус питомца при усыновлении.
    /// </summary>
    public Event<PetAdoptionFailed> PetAdoptionFailedEvent { get; private set; } = null!;

    /// <summary>
    /// Событие: бронирование питомца отменено (результат компенсации).
    /// </summary>
    public Event<PetUnreserved> PetUnreservedEvent { get; private set; } = null!;

    /// <summary>
    /// Конфигурация переходов и действий машины состояний.
    /// </summary>
    public PetAdoptionStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => StartPetAdoptionEvent, x => x.CorrelateById(m => m.Message.CorrelationId)
            .SelectId(context => context.Message.CorrelationId));
        Event(() => PetReservedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PetReservationFailedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AdoptionChatCreatedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AdoptionChatCreationFailedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => ConfirmAdoptionEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => RejectAdoptionEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PetAdoptedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PetAdoptionFailedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PetUnreservedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(StartPetAdoptionEvent)
                .Then(context =>
                {
                    var msg = context.Message;
                    context.Saga.PetId = msg.PetId;
                    context.Saga.VolunteerId = msg.VolunteerId;
                    context.Saga.AdopterId = msg.AdopterId;
                    context.Saga.AdopterName = msg.AdopterName;
                    context.Saga.PetNickName = msg.PetNickName;
                    context.Saga.CreatedAt = DateTime.UtcNow;
                    context.Saga.UpdatedAt = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<ReservePet>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.PetId,
                    context.Saga.VolunteerId,
                    context.Saga.AdopterId
                }))
                .TransitionTo(Reserving)
        );

        During(Reserving,
            When(PetReservedEvent)
                .Then(context => context.Saga.UpdatedAt = DateTime.UtcNow)
                .PublishAsync(context => context.Init<CreateAdoptionChat>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.PetId,
                    context.Saga.VolunteerId,
                    context.Saga.AdopterId,
                    context.Saga.PetNickName
                }))
                .TransitionTo(CreatingChat),
            When(PetReservationFailedEvent)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                    context.Saga.UpdatedAt = DateTime.UtcNow;
                })
                .Finalize()
        );

        During(CreatingChat,
            When(AdoptionChatCreatedEvent)
                .Then(context =>
                {
                    context.Saga.ChatId = context.Message.ChatId;
                    context.Saga.UpdatedAt = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<NotificationEvent>(new
                {
                    EventId = Guid.NewGuid(),
                    UserId = context.Saga.AdopterId,
                    Message = $"Ваш запрос на усыновление питомца \"{context.Saga.PetNickName}\" принят. " +
                              $"Чат с волонтёром создан.",
                    NotificationType = "Email"
                }))
                .PublishAsync(context => context.Init<NotificationEvent>(new
                {
                    EventId = Guid.NewGuid(),
                    UserId = context.Saga.VolunteerId,
                    Message =
                        $"Пользователь \"{context.Saga.AdopterName}\" хочет усыновить питомца " +
                        $"\"{context.Saga.PetNickName}\". Чат создан.",
                    NotificationType = "Email"
                }))
                .TransitionTo(WaitingForAdoption),
            When(AdoptionChatCreationFailedEvent)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                    context.Saga.UpdatedAt = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<UnreservePet>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.PetId,
                    context.Saga.VolunteerId
                }))
                .TransitionTo(Compensating)
        );

        During(WaitingForAdoption,
            When(ConfirmAdoptionEvent)
                .Then(context => context.Saga.UpdatedAt = DateTime.UtcNow)
                .PublishAsync(context => context.Init<AdoptPet>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.PetId,
                    context.Saga.VolunteerId,
                    context.Saga.AdopterId
                }))
                .TransitionTo(Adopting),
            When(RejectAdoptionEvent)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason ?? "Волонтёр отклонил усыновление";
                    context.Saga.UpdatedAt = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<UnreservePet>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.PetId,
                    context.Saga.VolunteerId
                }))
                .TransitionTo(Compensating)
        );

        During(Adopting,
            When(PetAdoptedEvent)
                .Then(context => context.Saga.UpdatedAt = DateTime.UtcNow)
                .PublishAsync(context => context.Init<NotificationEvent>(new
                {
                    EventId = Guid.NewGuid(),
                    UserId = context.Saga.AdopterId,
                    Message =
                        $"Поздравляем! Усыновление питомца \"{context.Saga.PetNickName}\" подтверждено волонтёром.",
                    NotificationType = "Email"
                }))
                .Finalize(),
            When(PetAdoptionFailedEvent)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                    context.Saga.UpdatedAt = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<UnreservePet>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.PetId,
                    context.Saga.VolunteerId
                }))
                .TransitionTo(Compensating)
        );

        During(Compensating,
            When(PetUnreservedEvent)
                .Then(context => context.Saga.UpdatedAt = DateTime.UtcNow)
                .Finalize()
        );
    }
}