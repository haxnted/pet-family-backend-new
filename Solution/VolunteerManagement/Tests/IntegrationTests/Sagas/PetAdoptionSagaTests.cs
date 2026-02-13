using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Notification.Contracts.Events;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using VolunteerManagement.Infrastructure.SagaStates;
using VolunteerManagement.Services.Sagas;

namespace VolunteerManagement.Tests.Integration.Sagas;

/// <summary>
/// Тесты машины состояний саги PetAdoptionStateMachine.
/// Используется MassTransit TestHarness с in-memory транспортом.
/// </summary>
public class PetAdoptionSagaTests : IAsyncLifetime
{
    private ServiceProvider _provider = null!;
    private ITestHarness _harness = null!;
    private ISagaStateMachineTestHarness<PetAdoptionStateMachine, PetAdoptionState> _sagaHarness = null!;

    private readonly Guid _correlationId = Guid.NewGuid();
    private readonly Guid _petId = Guid.NewGuid();
    private readonly Guid _volunteerId = Guid.NewGuid();
    private readonly Guid _adopterId = Guid.NewGuid();
    private const string AdopterName = "Иван Иванов";
    private const string PetNickName = "Барсик";

    public async Task InitializeAsync()
    {
        _provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSagaStateMachine<PetAdoptionStateMachine, PetAdoptionState>()
                    .InMemoryRepository();
            })
            .BuildServiceProvider(true);

        _harness = _provider.GetRequiredService<ITestHarness>();
        await _harness.Start();

        _sagaHarness = _harness
            .GetSagaStateMachineHarness<PetAdoptionStateMachine, PetAdoptionState>();
    }

    public async Task DisposeAsync()
    {
        await _harness.Stop();
        await _provider.DisposeAsync();
    }

    #region Helper Methods

    private StartPetAdoption CreateStartPetAdoption() => new()
    {
        CorrelationId = _correlationId,
        PetId = _petId,
        VolunteerId = _volunteerId,
        AdopterId = _adopterId,
        AdopterName = AdopterName,
        PetNickName = PetNickName
    };

    private async Task BringToReservingState()
    {
        await _harness.Bus.Publish(CreateStartPetAdoption());
        await _sagaHarness.Exists(_correlationId, x => x.Reserving, timeout: TimeSpan.FromSeconds(5));
    }

    private async Task BringToCreatingChatState()
    {
        await BringToReservingState();
        await _harness.Bus.Publish(new PetReserved
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            VolunteerId = _volunteerId,
            AdopterId = _adopterId
        });
        await _sagaHarness.Exists(_correlationId, x => x.CreatingChat, timeout: TimeSpan.FromSeconds(5));
    }

    private async Task BringToWaitingForAdoptionState()
    {
        await BringToCreatingChatState();
        var chatId = Guid.NewGuid();
        await _harness.Bus.Publish(new AdoptionChatCreated
        {
            CorrelationId = _correlationId,
            ChatId = chatId,
            PetId = _petId
        });
        await _sagaHarness.Exists(_correlationId, x => x.WaitingForAdoption, timeout: TimeSpan.FromSeconds(5));
    }

    private async Task BringToAdoptingState()
    {
        await BringToWaitingForAdoptionState();
        await _harness.Bus.Publish(new ConfirmAdoption
        {
            CorrelationId = _correlationId,
            VolunteerId = _volunteerId
        });
        await _sagaHarness.Exists(_correlationId, x => x.Adopting, timeout: TimeSpan.FromSeconds(5));
    }

    #endregion

    #region Initially → Reserving

    [Fact]
    public async Task StartPetAdoption_ShouldTransitionToReserving_AndPublishReservePet()
    {
        // Act
        await _harness.Bus.Publish(CreateStartPetAdoption());

        // Assert — saga transitioned to Reserving
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.Reserving,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — saga state populated correctly
        var instance = _sagaHarness.Sagas.ContainsInState(_correlationId, _sagaHarness.StateMachine, x => x.Reserving);
        instance.Should().NotBeNull();
        instance!.PetId.Should().Be(_petId);
        instance.VolunteerId.Should().Be(_volunteerId);
        instance.AdopterId.Should().Be(_adopterId);
        instance.AdopterName.Should().Be(AdopterName);
        instance.PetNickName.Should().Be(PetNickName);
        instance.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        // Assert — ReservePet command published
        (await _harness.Published.Any<ReservePet>(
            x => x.Context.Message.CorrelationId == _correlationId
                 && x.Context.Message.PetId == _petId))
            .Should().BeTrue();
    }

    #endregion

    #region Reserving → CreatingChat

    [Fact]
    public async Task PetReserved_ShouldTransitionToCreatingChat_AndPublishCreateAdoptionChat()
    {
        // Arrange
        await BringToReservingState();

        // Act
        await _harness.Bus.Publish(new PetReserved
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            VolunteerId = _volunteerId,
            AdopterId = _adopterId
        });

        // Assert — saga transitioned to CreatingChat
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.CreatingChat,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — CreateAdoptionChat command published
        (await _harness.Published.Any<CreateAdoptionChat>(
            x => x.Context.Message.CorrelationId == _correlationId
                 && x.Context.Message.PetId == _petId
                 && x.Context.Message.VolunteerId == _volunteerId
                 && x.Context.Message.AdopterId == _adopterId
                 && x.Context.Message.PetNickName == PetNickName))
            .Should().BeTrue();
    }

    #endregion

    #region Reserving → Final (reservation failed)

    [Fact]
    public async Task PetReservationFailed_ShouldFinalize_WithFailureReason()
    {
        // Arrange
        await BringToReservingState();
        var reason = "Питомец уже забронирован";

        // Act
        await _harness.Bus.Publish(new PetReservationFailed
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            Reason = reason
        });

        // Assert — saga finalized (no longer exists in active state)
        var notExists = await _sagaHarness.NotExists(_correlationId,
            timeout: TimeSpan.FromSeconds(5));
        notExists.HasValue.Should().BeTrue();
    }

    #endregion

    #region CreatingChat → WaitingForAdoption

    [Fact]
    public async Task AdoptionChatCreated_ShouldTransitionToWaitingForAdoption_AndSendNotifications()
    {
        // Arrange
        await BringToCreatingChatState();
        var chatId = Guid.NewGuid();

        // Act
        await _harness.Bus.Publish(new AdoptionChatCreated
        {
            CorrelationId = _correlationId,
            ChatId = chatId,
            PetId = _petId
        });

        // Assert — saga transitioned to WaitingForAdoption
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.WaitingForAdoption,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — ChatId stored in saga state
        var instance = _sagaHarness.Sagas.ContainsInState(
            _correlationId, _sagaHarness.StateMachine, x => x.WaitingForAdoption);
        instance.Should().NotBeNull();
        instance!.ChatId.Should().Be(chatId);

        // Assert — two NotificationEvent messages published (adopter + volunteer)
        var notifications = _harness.Published
            .Select<NotificationEvent>()
            .Where(x => x.Context.Message.NotificationType == "Email")
            .ToList();

        notifications.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    #endregion

    #region CreatingChat → Compensating (chat creation failed)

    [Fact]
    public async Task AdoptionChatCreationFailed_ShouldCompensate_AndPublishUnreservePet()
    {
        // Arrange
        await BringToCreatingChatState();
        var reason = "Сервис чатов недоступен";

        // Act
        await _harness.Bus.Publish(new AdoptionChatCreationFailed
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            Reason = reason
        });

        // Assert — saga transitioned to Compensating
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.Compensating,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — failure reason stored
        var instance = _sagaHarness.Sagas.ContainsInState(
            _correlationId, _sagaHarness.StateMachine, x => x.Compensating);
        instance.Should().NotBeNull();
        instance!.FailureReason.Should().Be(reason);

        // Assert — UnreservePet compensation command published
        (await _harness.Published.Any<UnreservePet>(
            x => x.Context.Message.CorrelationId == _correlationId
                 && x.Context.Message.PetId == _petId))
            .Should().BeTrue();
    }

    #endregion

    #region WaitingForAdoption → Adopting (volunteer confirms)

    [Fact]
    public async Task ConfirmAdoption_ShouldTransitionToAdopting_AndPublishAdoptPet()
    {
        // Arrange
        await BringToWaitingForAdoptionState();

        // Act
        await _harness.Bus.Publish(new ConfirmAdoption
        {
            CorrelationId = _correlationId,
            VolunteerId = _volunteerId
        });

        // Assert — saga transitioned to Adopting
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.Adopting,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — AdoptPet command published
        (await _harness.Published.Any<AdoptPet>(
            x => x.Context.Message.CorrelationId == _correlationId
                 && x.Context.Message.PetId == _petId
                 && x.Context.Message.VolunteerId == _volunteerId
                 && x.Context.Message.AdopterId == _adopterId))
            .Should().BeTrue();
    }

    #endregion

    #region WaitingForAdoption → Compensating (volunteer rejects)

    [Fact]
    public async Task RejectAdoption_ShouldTransitionToCompensating_AndPublishUnreservePet()
    {
        // Arrange
        await BringToWaitingForAdoptionState();
        var reason = "Не подходит по условиям содержания";

        // Act
        await _harness.Bus.Publish(new RejectAdoption
        {
            CorrelationId = _correlationId,
            VolunteerId = _volunteerId,
            Reason = reason
        });

        // Assert — saga transitioned to Compensating
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.Compensating,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — failure reason stored
        var instance = _sagaHarness.Sagas.ContainsInState(
            _correlationId, _sagaHarness.StateMachine, x => x.Compensating);
        instance.Should().NotBeNull();
        instance!.FailureReason.Should().Be(reason);

        // Assert — UnreservePet compensation published
        (await _harness.Published.Any<UnreservePet>(
            x => x.Context.Message.CorrelationId == _correlationId
                 && x.Context.Message.PetId == _petId))
            .Should().BeTrue();
    }

    [Fact]
    public async Task RejectAdoption_WithoutReason_ShouldUseDefaultMessage()
    {
        // Arrange
        await BringToWaitingForAdoptionState();

        // Act
        await _harness.Bus.Publish(new RejectAdoption
        {
            CorrelationId = _correlationId,
            VolunteerId = _volunteerId,
            Reason = null
        });

        // Assert — default failure reason
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.Compensating,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        var instance = _sagaHarness.Sagas.ContainsInState(
            _correlationId, _sagaHarness.StateMachine, x => x.Compensating);
        instance.Should().NotBeNull();
        instance!.FailureReason.Should().Be("Волонтёр отклонил усыновление");
    }

    #endregion

    #region Adopting → Final (pet adopted)

    [Fact]
    public async Task PetAdopted_ShouldFinalize_AndSendNotification()
    {
        // Arrange
        await BringToAdoptingState();

        // Act
        await _harness.Bus.Publish(new PetAdopted
        {
            CorrelationId = _correlationId,
            PetId = _petId
        });

        // Assert — saga finalized
        var notExists = await _sagaHarness.NotExists(_correlationId,
            timeout: TimeSpan.FromSeconds(5));
        notExists.HasValue.Should().BeTrue();

        // Assert — congratulations notification published
        (await _harness.Published.Any<NotificationEvent>(
            x => x.Context.Message.UserId == _adopterId
                 && x.Context.Message.NotificationType == "Email"))
            .Should().BeTrue();
    }

    #endregion

    #region Adopting → Compensating (adoption failed)

    [Fact]
    public async Task PetAdoptionFailed_ShouldCompensate_AndPublishUnreservePet()
    {
        // Arrange
        await BringToAdoptingState();
        var reason = "Ошибка обновления статуса питомца";

        // Act
        await _harness.Bus.Publish(new PetAdoptionFailed
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            Reason = reason
        });

        // Assert — saga transitioned to Compensating
        var existsId = await _sagaHarness.Exists(_correlationId, x => x.Compensating,
            timeout: TimeSpan.FromSeconds(5));
        existsId.HasValue.Should().BeTrue();

        // Assert — failure reason stored
        var instance = _sagaHarness.Sagas.ContainsInState(
            _correlationId, _sagaHarness.StateMachine, x => x.Compensating);
        instance.Should().NotBeNull();
        instance!.FailureReason.Should().Be(reason);

        // Assert — UnreservePet compensation published
        (await _harness.Published.Any<UnreservePet>(
            x => x.Context.Message.CorrelationId == _correlationId
                 && x.Context.Message.PetId == _petId))
            .Should().BeTrue();
    }

    #endregion

    #region Compensating → Final

    [Fact]
    public async Task PetUnreserved_ShouldFinalizeSaga()
    {
        // Arrange — bring to Compensating via chat creation failure
        await BringToCreatingChatState();
        await _harness.Bus.Publish(new AdoptionChatCreationFailed
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            Reason = "Сервис недоступен"
        });
        await _sagaHarness.Exists(_correlationId, x => x.Compensating,
            timeout: TimeSpan.FromSeconds(5));

        // Act
        await _harness.Bus.Publish(new PetUnreserved
        {
            CorrelationId = _correlationId,
            PetId = _petId
        });

        // Assert — saga finalized
        var notExists = await _sagaHarness.NotExists(_correlationId,
            timeout: TimeSpan.FromSeconds(5));
        notExists.HasValue.Should().BeTrue();
    }

    #endregion

    #region End-to-End Scenarios

    [Fact]
    public async Task FullHappyPath_ShouldCompleteSuccessfully()
    {
        var chatId = Guid.NewGuid();

        // Step 1: Start adoption
        await _harness.Bus.Publish(CreateStartPetAdoption());
        (await _sagaHarness.Exists(_correlationId, x => x.Reserving,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Step 2: Pet reserved
        await _harness.Bus.Publish(new PetReserved
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            VolunteerId = _volunteerId,
            AdopterId = _adopterId
        });
        (await _sagaHarness.Exists(_correlationId, x => x.CreatingChat,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Step 3: Chat created
        await _harness.Bus.Publish(new AdoptionChatCreated
        {
            CorrelationId = _correlationId,
            ChatId = chatId,
            PetId = _petId
        });
        (await _sagaHarness.Exists(_correlationId, x => x.WaitingForAdoption,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Step 4: Volunteer confirms
        await _harness.Bus.Publish(new ConfirmAdoption
        {
            CorrelationId = _correlationId,
            VolunteerId = _volunteerId
        });
        (await _sagaHarness.Exists(_correlationId, x => x.Adopting,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Step 5: Pet adopted
        await _harness.Bus.Publish(new PetAdopted
        {
            CorrelationId = _correlationId,
            PetId = _petId
        });

        // Assert — saga finalized
        (await _sagaHarness.NotExists(_correlationId,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Assert — all expected messages were published
        (await _harness.Published.Any<ReservePet>()).Should().BeTrue();
        (await _harness.Published.Any<CreateAdoptionChat>()).Should().BeTrue();
        (await _harness.Published.Any<AdoptPet>()).Should().BeTrue();
    }

    [Fact]
    public async Task FullCompensationPath_RejectAdoption_ShouldRollbackCorrectly()
    {
        var chatId = Guid.NewGuid();

        // Steps 1-3: Bring to WaitingForAdoption
        await _harness.Bus.Publish(CreateStartPetAdoption());
        (await _sagaHarness.Exists(_correlationId, x => x.Reserving,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        await _harness.Bus.Publish(new PetReserved
        {
            CorrelationId = _correlationId,
            PetId = _petId,
            VolunteerId = _volunteerId,
            AdopterId = _adopterId
        });
        (await _sagaHarness.Exists(_correlationId, x => x.CreatingChat,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        await _harness.Bus.Publish(new AdoptionChatCreated
        {
            CorrelationId = _correlationId,
            ChatId = chatId,
            PetId = _petId
        });
        (await _sagaHarness.Exists(_correlationId, x => x.WaitingForAdoption,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Step 4: Volunteer rejects
        await _harness.Bus.Publish(new RejectAdoption
        {
            CorrelationId = _correlationId,
            VolunteerId = _volunteerId,
            Reason = "Не подходит"
        });
        (await _sagaHarness.Exists(_correlationId, x => x.Compensating,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Step 5: Pet unreserved (compensation completes)
        await _harness.Bus.Publish(new PetUnreserved
        {
            CorrelationId = _correlationId,
            PetId = _petId
        });

        // Assert — saga finalized
        (await _sagaHarness.NotExists(_correlationId,
            timeout: TimeSpan.FromSeconds(5))).HasValue.Should().BeTrue();

        // Assert — compensation messages were published
        (await _harness.Published.Any<UnreservePet>(
            x => x.Context.Message.CorrelationId == _correlationId))
            .Should().BeTrue();
    }

    #endregion
}
