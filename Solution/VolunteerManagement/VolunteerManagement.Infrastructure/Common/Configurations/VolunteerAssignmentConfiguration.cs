using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Domain.Aggregates.Shelters.Entities;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="VolunteerAssignment"/>.
/// </summary>
public class VolunteerAssignmentConfiguration : IEntityTypeConfiguration<VolunteerAssignment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VolunteerAssignment> builder)
    {
        builder.ToTable("VolunteerAssignments");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerAssignmentId.Of(value))
            .ValueGeneratedNever();

        builder.Property(a => a.VolunteerId)
            .HasColumnName("VolunteerId")
            .IsRequired();

        builder.Property(a => a.Role)
            .HasConversion<string>()
            .HasColumnName("Role")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.AssignedAt)
            .HasColumnName("AssignedAt")
            .IsRequired();

        builder.Property(a => a.IsActive)
            .HasColumnName("IsActive")
            .IsRequired();

        builder.HasIndex(a => a.VolunteerId);
    }
}
