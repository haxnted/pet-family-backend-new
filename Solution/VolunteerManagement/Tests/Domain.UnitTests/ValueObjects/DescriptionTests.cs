using FluentAssertions;
using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Tests.Domain.ValueObjects;

public sealed class DescriptionTests : UnitTestBase
{
    [Fact]
    public void Of_WithValidDescription_ShouldCreateDescription()
    {
        var validDescription = "Это валидное описание достаточной длины для теста";

        var description = Description.Of(validDescription);

        description.Should().NotBeNull();
        description.Value.Should().Be(validDescription);
    }

    [Fact]
    public void Of_WithMinimumLength_ShouldCreateDescription()
    {
        var minLengthDescription = new string('А', Description.MinLength);

        var description = Description.Of(minLengthDescription);

        description.Value.Should().HaveLength(Description.MinLength);
    }

    [Fact]
    public void Of_WithMaximumLength_ShouldCreateDescription()
    {
        var maxLengthDescription = new string('А', Description.MaxLength);

        var description = Description.Of(maxLengthDescription);

        description.Value.Should().HaveLength(Description.MaxLength);
    }

    [Fact]
    public void Of_WithTooShortDescription_ShouldThrowException()
    {
        var tooShortDescription = new string('А', Description.MinLength - 1);

        var act = () => Description.Of(tooShortDescription);

        act.Should().Throw<DomainException>()
            .WithMessage($"*{Description.MinLength}*");
    }

    [Fact]
    public void Of_WithTooLongDescription_ShouldThrowException()
    {
        var tooLongDescription = new string('А', Description.MaxLength + 1);

        var act = () => Description.Of(tooLongDescription);

        act.Should().Throw<DomainException>()
            .WithMessage($"*{Description.MaxLength}*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Of_WithEmptyOrNullDescription_ShouldThrowException(string? invalidDescription)
    {
        var act = () => Description.Of(invalidDescription!);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Of_ShouldTrimWhitespace()
    {
        var descriptionWithSpaces = "   Это описание с пробелами по краям   ";

        var description = Description.Of(descriptionWithSpaces);

        description.Value.Should().Be("Это описание с пробелами по краям");
    }

    [Fact]
    public void Equals_WithSameValues_ShouldBeEqual()
    {
        var description1 = Description.Of("Одинаковое описание для теста");
        var description2 = Description.Of("Одинаковое описание для теста");

        description1.Should().Be(description2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldNotBeEqual()
    {
        var description1 = Description.Of("Первое описание для теста");
        var description2 = Description.Of("Второе описание для теста");

        description1.Should().NotBe(description2);
    }
}
