using Ardalis.Specification;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PetFamily.SharedKernel.Application.Exceptions;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using PetFamily.SharedKernel.Infrastructure.Caching;
using PetFamily.SharedKernel.Tests.Abstractions;
using PetFamily.SharedKernel.Tests.Fakes;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Application.Services;

public sealed class VolunteerServiceTests : UnitTestBase
{
    private readonly IRepository<Volunteer> _repositoryMock;
    private readonly ICacheService _cacheMock;
    private readonly IVolunteerService _sut;

    public VolunteerServiceTests()
    {
        _repositoryMock = Substitute.For<IRepository<Volunteer>>();
        _cacheMock = Substitute.For<ICacheService>();
        _sut = CreateVolunteerService(_repositoryMock, _cacheMock);
    }

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_WithValidData_ShouldAddVolunteerToRepository()
    {
        var name = FakeDataGenerator.FirstName();
        var surname = FakeDataGenerator.LastName();
        var patronymic = FakeDataGenerator.Patronymic();
        var userId = Guid.NewGuid();

        Volunteer? capturedVolunteer = null;
        _repositoryMock.AddAsync(Arg.Do<Volunteer>(v => capturedVolunteer = v), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await _sut.AddAsync(name, surname, patronymic, userId, CancellationToken.None);

        await _repositoryMock.Received(1).AddAsync(Arg.Any<Volunteer>(), Arg.Any<CancellationToken>());
        capturedVolunteer.Should().NotBeNull();
        capturedVolunteer!.FullName.Name.Should().Be(name);
        capturedVolunteer.FullName.Surname.Should().Be(surname);
        capturedVolunteer.UserId.Should().Be(userId);
    }

    #endregion

    #region GetAsync Tests

    [Fact]
    public async Task GetAsync_WithExistingVolunteer_ShouldReturnVolunteer()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        _repositoryMock.FirstOrDefaultAsync(Arg.Any<ISpecification<Volunteer>>(), Arg.Any<CancellationToken>())
            .Returns(volunteer);

        var result = await _sut.GetAsync(volunteer.Id.Value, CancellationToken.None);

        result.Should().Be(volunteer);
    }

    [Fact]
    public async Task GetAsync_WithNonExistentVolunteer_ShouldThrowArgumentNullException()
    {
        _repositoryMock.FirstOrDefaultAsync(Arg.Any<ISpecification<Volunteer>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        var act = async () => await _sut.GetAsync(Guid.NewGuid(), CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException<Volunteer>>();
    }

    #endregion

    #region SoftRemoveAsync Tests

    [Fact]
    public async Task SoftRemoveAsync_WithExistingVolunteer_ShouldMarkAsDeleted()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        _repositoryMock.FirstOrDefaultAsync(Arg.Any<ISpecification<Volunteer>>(), Arg.Any<CancellationToken>())
            .Returns(volunteer);
        _repositoryMock.UpdateAsync(Arg.Any<Volunteer>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await _sut.SoftRemoveAsync(volunteer.Id.Value, CancellationToken.None);

        volunteer.IsDeleted.Should().BeTrue();
        await _repositoryMock.Received(1).UpdateAsync(volunteer, Arg.Any<CancellationToken>());
    }

    #endregion

    #region GetWithPaginationAsync Tests

    [Fact]
    public async Task GetWithPaginationAsync_ShouldReturnPaginatedVolunteers()
    {
        var volunteers = VolunteerBuilder.BuildMany(10);
        _repositoryMock.GetAll(Arg.Any<ISpecification<Volunteer>>(), Arg.Any<CancellationToken>())
            .Returns(volunteers);

        var result = await _sut.GetWithPaginationAsync(1, 10, CancellationToken.None);

        result.Should().HaveCount(10);
        await _repositoryMock.Received(1).GetAll(Arg.Any<ISpecification<Volunteer>>(), Arg.Any<CancellationToken>());
    }

    #endregion

    private static IVolunteerService CreateVolunteerService(
        IRepository<Volunteer> repository,
        ICacheService cache)
    {
        var serviceType = typeof(IVolunteerService).Assembly
            .GetTypes()
            .First(t => t.Name == "VolunteerService" && !t.IsInterface);

        var instance = Activator.CreateInstance(serviceType, repository, cache);
        return (IVolunteerService)instance!;
    }
}
