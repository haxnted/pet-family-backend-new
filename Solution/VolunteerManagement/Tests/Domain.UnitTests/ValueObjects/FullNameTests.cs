using FluentAssertions;
using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Tests.Domain.ValueObjects;

public sealed class FullNameTests : UnitTestBase
{
    [Theory]
    [InlineData("Иван", "Иванов", null)]
    [InlineData("Петр", "Петров", "Петрович")]
    [InlineData("Anna", "Smith", "Marie")]
    public void Of_WithValidData_ShouldCreateFullName(string name, string surname, string? patronymic)
    {
        var fullName = FullName.Of(name, surname, patronymic);

        fullName.Should().NotBeNull();
        fullName.Name.Should().Be(name);
        fullName.Surname.Should().Be(surname);
        fullName.Patronymic.Should().Be(patronymic);
    }

    [Theory]
    [InlineData("", "Иванов", null)]
    [InlineData("   ", "Иванов", null)]
    [InlineData(null, "Иванов", null)]
    public void Of_WithEmptyName_ShouldThrowException(string? name, string surname, string? patronymic)
    {
        var act = () => FullName.Of(name!, surname, patronymic);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("Иван", "", null)]
    [InlineData("Иван", "   ", null)]
    [InlineData("Иван", null, null)]
    public void Of_WithEmptySurname_ShouldThrowException(string name, string? surname, string? patronymic)
    {
        var act = () => FullName.Of(name, surname!, patronymic);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Of_WithTooLongName_ShouldThrowException()
    {
        var tooLongName = new string('А', FullName.MaxLength + 1);

        var act = () => FullName.Of(tooLongName, "Иванов", null);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Of_WithTooLongPatronymic_ShouldThrowException()
    {
        var tooLongPatronymic = new string('А', 51);

        var act = () => FullName.Of("Иван", "Иванов", tooLongPatronymic);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Of_ShouldTrimWhitespace()
    {
        var fullName = FullName.Of("  Иван  ", "  Иванов  ", "  Иванович  ");

        fullName.Name.Should().Be("Иван");
        fullName.Surname.Should().Be("Иванов");
        fullName.Patronymic.Should().Be("Иванович");
    }

    [Fact]
    public void Equals_WithSameValues_ShouldBeEqual()
    {
        var fullName1 = FullName.Of("Иван", "Иванов", "Иванович");
        var fullName2 = FullName.Of("Иван", "Иванов", "Иванович");

        fullName1.Should().Be(fullName2);
        (fullName1 == fullName2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldNotBeEqual()
    {
        var fullName1 = FullName.Of("Иван", "Иванов", "Иванович");
        var fullName2 = FullName.Of("Петр", "Петров", "Петрович");

        fullName1.Should().NotBe(fullName2);
        (fullName1 != fullName2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        var fullName = FullName.Of("Иван", "Иванов", "Иванович");

        var result = fullName.ToString();

        result.Should().Be("Иванов Иван Иванович");
    }

    [Fact]
    public void CompareTo_ShouldCompareCorrectly()
    {
        var fullName1 = FullName.Of("Александр", "Александров", null);
        var fullName2 = FullName.Of("Борис", "Борисов", null);

        fullName1.CompareTo(fullName2).Should().BeLessThan(0);
        fullName2.CompareTo(fullName1).Should().BeGreaterThan(0);
    }
}
