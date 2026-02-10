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