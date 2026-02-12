using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerManagement.Infrastructure.SagaStates;

namespace VolunteerManagement.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация EF Core для состояния саги усыновления.
/// </summary>
public class PetAdoptionStateConfiguration : IEntityTypeConfiguration<PetAdoptionState>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PetAdoptionState> builder)
    {
        builder.ToTable("PetAdoptionStates");

        builder.HasKey(x => x.CorrelationId);

        builder.Property(x => x.CurrentState)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.PetId).IsRequired();
        builder.Property(x => x.VolunteerId).IsRequired();
        builder.Property(x => x.AdopterId).IsRequired();

        builder.Property(x => x.AdopterName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.PetNickName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ChatId).IsRequired(false);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.Property(x => x.FailureReason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasIndex(x => x.PetId);
        builder.HasIndex(x => x.AdopterId);
        builder.HasIndex(x => x.CurrentState);
    }
}
