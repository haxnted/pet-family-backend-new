using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация агрегата <see cref="Volunteer"/>.
/// </summary>
public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("Volunteers");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Of(value))
            .ValueGeneratedNever();

        builder.OwnsOne(v => v.FullName, fullName =>
        {
            fullName.Property(fn => fn.Name)
                .HasColumnName("Name")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();

            fullName.Property(fn => fn.Surname)
                .HasColumnName("Surname")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();

            fullName.Property(fn => fn.Patronymic)
                .HasColumnName("Patronymic")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired(false);
        });

        builder.Property(v => v.GeneralDescription)
            .HasConversion(
                desc => desc.Value,
                value => Description.Of(value))
            .HasColumnName("GeneralDescription")
            .HasMaxLength(Description.MaxLength)
            .IsRequired();

        builder.Property(v => v.AgeExperience)
            .HasConversion(
                exp => exp != null ? exp.Value : (int?)null,
                value => value.HasValue ? AgeExperience.Of(value.Value) : null)
            .HasColumnName("AgeExperience")
            .IsRequired(false);

        builder.Property(v => v.PhoneNumber)
            .HasConversion(
                phone => phone != null ? phone.Value : null,
                value => value != null ? PhoneNumber.Of(value) : null)
            .HasColumnName("PhoneNumber")
            .HasMaxLength(11)
            .IsRequired(false);

        builder.Property(v => v.UserId)
            .HasColumnName("UserId")
            .IsRequired(false);

        builder.Property(v => v.Photo)
            .HasConversion(
                photo => photo != null ? photo.Value : (Guid?)null,
                value => value.HasValue ? Photo.Create(value.Value) : null)
            .HasColumnName("Photo")
            .IsRequired(false);

        builder.HasIndex(v => v.UserId)
            .IsUnique(false);

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(v => v.VolunteerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Pets)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}