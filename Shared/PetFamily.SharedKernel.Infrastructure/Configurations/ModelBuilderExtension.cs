using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetFamily.SharedKernel.Infrastructure.Configurations.Converters;

namespace PetFamily.SharedKernel.Infrastructure.Configurations;

/// <summary>
/// Расширение для сборщика моделей.
/// </summary>
public static class ModelBuilderExtension
{
    /// <summary>
    /// Устанавливает тип дат по-умолчанию.
    /// </summary>
    /// <param name="modelBuilder">Сборщик моделей.</param>
    /// <param name="kind">Тип дат.</param>
    public static void SetDefaultDateTimeKind(this ModelBuilder modelBuilder, DateTimeKind kind)
    {
        var converter = kind switch
        {
            DateTimeKind.Utc => DateTimeKindValueConverter.Utc,
            DateTimeKind.Local => DateTimeKindValueConverter.Local,
            DateTimeKind.Unspecified => DateTimeKindValueConverter.Unspecified,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported DateTimeKind")
        };

        modelBuilder.UseValueConverterForType<DateTime>(converter);
        modelBuilder.UseValueConverterForType<DateTime?>(converter);
    }

    private static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter)
    {
        return modelBuilder.UseValueConverterForType(typeof(T), converter);
    }

    internal static ModelBuilder UseValueConverterForType(this ModelBuilder modelBuilder, Type type, ValueConverter converter)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(converter);
            }
        }

        return modelBuilder;
    }
}