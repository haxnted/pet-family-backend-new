using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;
using DomainSpecies = VolunteerManagement.Domain.Aggregates.AnimalKinds.Species;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности Species.
/// </summary>
public class SpeciesConfiguration : IEntityTypeConfiguration<DomainSpecies>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<DomainSpecies> builder)
    {
        builder.ToTable("Species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Of(value))
            .ValueGeneratedNever();

        builder.Property(s => s.AnimalKind)
            .HasConversion(
                animalKind => animalKind.Value,
                value => AnimalKind.Of(value))
            .HasColumnName("AnimalKind")
            .HasMaxLength(AnimalKind.MaxLength)
            .IsRequired();

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(b => b.SpeciesId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}