using Account.Domain.Aggregates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация агрегата <see cref="DomainAccount"/>.
/// </summary>
public class AccountConfiguration : IEntityTypeConfiguration<DomainAccount>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DomainAccount> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => AccountId.Of(value))
            .ValueGeneratedNever();

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.HasIndex(a => a.UserId)
            .IsUnique();

        builder.Property(a => a.PhoneNumber)
            .HasConversion(
                phone => phone != null ? phone.Value : null,
                value => value != null ? PhoneNumber.Of(value) : null)
            .HasMaxLength(PhoneNumber.MaxLength)
            .IsRequired(false);

        builder.Property(a => a.AgeExperience)
            .HasConversion(
                exp => exp != null ? exp.Value : (int?)null,
                value => value.HasValue ? AgeExperience.Of(value.Value) : null)
            .IsRequired(false);

        builder.Property(a => a.Description)
            .HasConversion(
                desc => desc != null ? desc.Value : null,
                value => value != null ? Description.Of(value) : null)
            .HasMaxLength(Description.MaxLength)
            .IsRequired(false);

        builder.Property(a => a.Photo)
            .HasConversion(
                photo => photo != null ? photo.Value : (Guid?)null,
                value => value.HasValue ? Photo.Create(value.Value) : null)
            .IsRequired(false);
    }
}
