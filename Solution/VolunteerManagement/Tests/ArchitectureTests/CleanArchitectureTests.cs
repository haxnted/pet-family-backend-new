using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Handlers.Volunteers.Commands.Add;
using VolunteerManagement.Infrastructure.Common.Contexts;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Tests.Architecture;

/// <summary>
/// Архитектурные тесты для проверки соблюдения Clean Architecture.
/// </summary>
public sealed class CleanArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(Volunteer).Assembly;
    private static readonly Assembly ServicesAssembly = typeof(IVolunteerService).Assembly;
    private static readonly Assembly HandlersAssembly = typeof(AddVolunteerHandler).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(VolunteerManagementDbContext).Assembly;

    private const string DomainNamespace = "VolunteerManagement.Domain";
    private const string ServicesNamespace = "VolunteerManagement.Services";
    private const string HandlersNamespace = "VolunteerManagement.Handlers";
    private const string InfrastructureNamespace = "VolunteerManagement.Infrastructure";
    private const string HostsNamespace = "VolunteerManagement.Hosts";

    #region Domain Layer Tests

    [Fact]
    public void Domain_ShouldNot_HaveDependencyOn_Application()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(ServicesNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain should not depend on Services. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependencyOn_Handlers()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(HandlersNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain should not depend on Handlers. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependencyOn_Infrastructure()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain should not depend on Infrastructure. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependencyOn_Hosts()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(HostsNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain should not depend on Hosts. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region Application Layer Tests

    [Fact]
    public void Services_ShouldNot_HaveDependencyOn_Infrastructure()
    {
        var result = Types
            .InAssembly(ServicesAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Services should not depend on Infrastructure. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Services_ShouldNot_HaveDependencyOn_Hosts()
    {
        var result = Types
            .InAssembly(ServicesAssembly)
            .ShouldNot()
            .HaveDependencyOn(HostsNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Services should not depend on Hosts. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Handlers_ShouldNot_HaveDependencyOn_Infrastructure()
    {
        var result = Types
            .InAssembly(HandlersAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Handlers should not depend on Infrastructure. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Handlers_ShouldNot_HaveDependencyOn_Hosts()
    {
        var result = Types
            .InAssembly(HandlersAssembly)
            .ShouldNot()
            .HaveDependencyOn(HostsNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Handlers should not depend on Hosts. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region Infrastructure Layer Tests

    [Fact]
    public void Infrastructure_ShouldNot_HaveDependencyOn_Hosts()
    {
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(HostsNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Infrastructure should not depend on Hosts. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Infrastructure_ShouldNot_HaveDependencyOn_Handlers()
    {
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(HandlersNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Infrastructure should not depend on Handlers. Failing types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion
}
