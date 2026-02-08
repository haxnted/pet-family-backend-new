using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности Breed.
/// </summary>
public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("Breeds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Of(value))
            .ValueGeneratedNever();

        builder.Property(b => b.SpeciesId)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Of(value))
            .IsRequired();

        builder.Property(b => b.Name)
            .HasConversion(
                name => name.Value,
                value => BreedName.Of(value))
            .HasColumnName("Name")
            .HasMaxLength(BreedName.MaxLength)
            .IsRequired();
    }
}