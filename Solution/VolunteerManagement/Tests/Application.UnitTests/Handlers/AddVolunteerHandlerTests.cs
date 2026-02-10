using NSubstitute.ExceptionExtensions;
using PetFamily.SharedKernel.Tests.Abstractions;
using PetFamily.SharedKernel.Tests.Fakes;
using VolunteerManagement.Handlers.Volunteers.Commands.Add;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Tests.Application.Handlers;

public sealed class AddVolunteerHandlerTests : UnitTestBase
{
    private readonly IVolunteerService _volunteerServiceMock;
    private readonly AddVolunteerHandler _sut;

    public AddVolunteerHandlerTests()
    {
        _volunteerServiceMock = Substitute.For<IVolunteerService>();
        _sut = new AddVolunteerHandler(_volunteerServiceMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallVolunteerService()
    {
        var command = new AddVolunteerCommand
        {
            Name = FakeDataGenerator.FirstName(),
            Surname = FakeDataGenerator.LastName(),
            Patronymic = FakeDataGenerator.Patronymic(),
            UserId = Guid.NewGuid(),
        };

        _volunteerServiceMock.AddAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await _sut.Handle(command, CancellationToken.None);

        await _volunteerServiceMock.Received(1).AddAsync(
            command.Name,
            command.Surname,
            command.Patronymic,
            command.UserId,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenServiceThrows_ShouldPropagateException()
    {
        var command = new AddVolunteerCommand
        {
            Name = FakeDataGenerator.FirstName(),
            Surname = FakeDataGenerator.LastName(),
            Patronymic = null,
            UserId = Guid.NewGuid(),
        };

        _volunteerServiceMock.AddAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Service error"));

        var act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Service error");
    }
}