using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация агрегата <see cref="Shelter"/>.
/// </summary>
public class ShelterConfiguration : IEntityTypeConfiguration<Shelter>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Shelter> builder)
    {
        builder.ToTable("Shelters");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ShelterId.Of(value))
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .HasConversion(
                name => name.Value,
                value => ShelterName.Of(value))
            .HasColumnName("Name")
            .HasMaxLength(ShelterName.MaxLength)
            .IsRequired();

        builder.OwnsOne(s => s.Address, address =>
        {
            address.Property(a => a.City)
                .HasColumnName("City")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.State)
                .HasColumnName("State")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.Street)
                .HasColumnName("Street")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.ZipCode)
                .HasColumnName("ZipCode")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.Property(s => s.PhoneNumber)
            .HasConversion(
                phone => phone.Value,
                value => PhoneNumber.Of(value))
            .HasColumnName("PhoneNumber")
            .HasMaxLength(PhoneNumber.MaxLength)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasConversion(
                desc => desc.Value,
                value => Description.Of(value))
            .HasColumnName("Description")
            .HasMaxLength(Description.MaxLength)
            .IsRequired();

        builder.OwnsOne(s => s.WorkingHours, wh =>
        {
            wh.Property(w => w.OpenTime)
                .HasColumnName("OpenTime")
                .IsRequired();

            wh.Property(w => w.CloseTime)
                .HasColumnName("CloseTime")
                .IsRequired();
        });

        builder.Property(s => s.Capacity)
            .HasColumnName("Capacity")
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasColumnName("Status")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(s => s.VolunteerAssignments)
            .WithOne()
            .HasForeignKey("ShelterId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.VolunteerAssignments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
