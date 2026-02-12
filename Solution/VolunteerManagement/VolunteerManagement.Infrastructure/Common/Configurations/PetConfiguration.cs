using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Pet"/>.
/// </summary>
public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id =>
                    id.Value,
                value => PetId.Of(value))
            .ValueGeneratedNever();

        builder.Property(p => p.VolunteerId)
            .HasConversion(id =>
                    id.Value,
                value => VolunteerId.Of(value))
            .IsRequired();

        builder.Property(p => p.NickName)
            .HasConversion(nick => nick.Value, value => NickName.Of(value))
            .HasColumnName("NickName")
            .HasMaxLength(NickName.MaxLength)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasConversion(desc => desc.Value, value => Description.Of(value))
            .HasColumnName("Description")
            .HasMaxLength(Description.MaxLength)
            .IsRequired();

        builder.Property(p => p.HealthInformation)
            .HasConversion(desc => desc.Value, value => Description.Of(value))
            .HasColumnName("HealthInformation")
            .HasMaxLength(Description.MaxLength)
            .IsRequired();

        builder.OwnsOne(p => p.PhysicalAttributes, attributes =>
        {
            attributes.Property(a => a.Weight)
                .HasColumnName("Weight")
                .HasPrecision(18, 2)
                .IsRequired();

            attributes.Property(a => a.Height)
                .HasColumnName("Height")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.Property(p => p.BreedId)
            .HasColumnName("BreedId")
            .IsRequired();

        builder.Property(p => p.SpeciesId)
            .HasColumnName("SpeciesId")
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .HasColumnName("BirthDate")
            .IsRequired();

        builder.Property(p => p.IsCastrated)
            .HasColumnName("IsCastrated")
            .IsRequired();

        builder.Property(p => p.IsVaccinated)
            .HasColumnName("IsVaccinated")
            .IsRequired();

        builder.Property(p => p.BookerId)
            .HasColumnName("BookerId")
            .IsRequired(false);

        builder.Property(p => p.HelpStatus)
            .HasConversion<string>()
            .HasColumnName("HelpStatus")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Position)
            .HasConversion(
                pos => pos.Value,
                value => Position.Of(value))
            .HasColumnName("Position")
            .IsRequired();

        builder.Property(p => p.DateCreated)
            .HasColumnName("DateCreated")
            .IsRequired();

        builder.OwnsMany(p => p.Photos, photo =>
        {
            photo.ToJson("PetPhotos");

            photo.Property(ph => ph.Value)
                .IsRequired();
        });

        builder.OwnsMany(p => p.RequisiteList, requisite =>
        {
            requisite.ToJson("Requisites");

            requisite.Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();

            requisite.Property(r => r.Description)
                .HasMaxLength(500)
                .IsRequired();
        });
    }
}