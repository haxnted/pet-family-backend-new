using System.Reflection;
using NetArchTest.Rules;
using PetFamily.SharedKernel.Domain.Primitives;
using VolunteerManagement.Domain.Aggregates.Volunteers;

namespace VolunteerManagement.Tests.Architecture;

/// <summary>
/// Архитектурные тесты для проверки соблюдения DDD паттернов в Domain слое.
/// </summary>
public sealed class DomainDesignTests
{
    private static readonly Assembly DomainAssembly = typeof(Volunteer).Assembly;

    #region Entity Tests

    [Fact]
    public void Entities_Should_BeSealed()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"All entities should be sealed. Non-sealed entities: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .And()
            .AreNotAbstract()
            .GetTypes();

        foreach (var entityType in entityTypes)
        {
            var hasPrivateParameterlessConstructor = entityType
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(c => c.GetParameters().Length <= 1);

            hasPrivateParameterlessConstructor.Should().BeTrue(
                $"Entity {entityType.Name} should have private constructor for EF Core");
        }
    }

    #endregion

    #region Value Object Tests

    [Fact]
    public void ValueObjects_Should_BeSealed()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(ValueObject))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"All value objects should be sealed. Non-sealed: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void ValueObjects_Should_BeImmutable()
    {
        var valueObjectTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(ValueObject))
            .And()
            .AreNotAbstract()
            .GetTypes();

        foreach (var voType in valueObjectTypes)
        {
            var properties = voType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var setter = property.GetSetMethod(true);
                var hasPublicSetter = setter?.IsPublic == true;

                hasPublicSetter.Should().BeFalse(
                    $"Value object {voType.Name}.{property.Name} should not have public setter");
            }
        }
    }

    [Fact]
    public void ValueObjects_Should_ResideInValueObjectsNamespace()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(ValueObject))
            .And()
            .AreNotAbstract()
            .Should()
            .ResideInNamespaceContaining("ValueObjects")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Value objects should reside in ValueObjects namespace. Misplaced: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region Exception Tests

    [Fact]
    public void DomainExceptions_Should_EndWithException()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Exception))
            .Should()
            .HaveNameEndingWith("Exception")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain exceptions should end with 'Exception'. Incorrectly named: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void DomainExceptions_Should_ResideInExceptionsNamespace()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Exception))
            .Should()
            .ResideInNamespaceContaining("Exceptions")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Exceptions should reside in Exceptions namespace. Misplaced: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region Naming Convention Tests

    [Fact]
    public void Interfaces_Should_StartWithI()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Interfaces should start with 'I'. Incorrectly named: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion
}
