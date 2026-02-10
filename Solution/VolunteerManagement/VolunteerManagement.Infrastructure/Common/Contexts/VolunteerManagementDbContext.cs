using MassTransit;
using Microsoft.EntityFrameworkCore;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using DomainSpecies = VolunteerManagement.Domain.Aggregates.AnimalKinds.Species;

namespace VolunteerManagement.Infrastructure.Common.Contexts;

/// <summary>
/// Контекст базы данных для работы с волонтерами.
/// </summary>
/// <param name="options">Опции контекста.</param>
public class VolunteerManagementDbContext(DbContextOptions<VolunteerManagementDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция волонтеров.
    /// </summary>
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();

    /// <summary>
    /// Коллекция видов животных.
    /// </summary>
    public DbSet<DomainSpecies> Species => Set<DomainSpecies>();

    /// <summary>
    /// Коллекция приютов.
    /// </summary>
    public DbSet<Shelter> Shelters => Set<Shelter>();

    /// <summary>
    /// Коллекция животных.
    /// </summary>
    public DbSet<Pet> Pets => Set<Pet>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CustomModelBuilder.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}