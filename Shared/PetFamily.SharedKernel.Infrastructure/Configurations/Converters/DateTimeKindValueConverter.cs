using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PetFamily.SharedKernel.Infrastructure.Configurations.Converters;

/// <summary>
/// Конвертер DateTime с явным указанием DateTimeKind.
/// Обеспечивает корректное преобразование DateTime при сохранении и загрузке из базы данных.
/// </summary>
public sealed class DateTimeKindValueConverter : ValueConverter<DateTime, DateTime>
{
    /// <summary>
    /// Инициализирует новый экземпляр конвертера с указанным DateTimeKind.
    /// </summary>
    /// <param name="kind">Тип DateTimeKind для преобразования.</param>
    /// <param name="mappingHints">Подсказки для маппинга (опционально).</param>
    private DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = null)
        : base(
            convertToProviderExpression: v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, kind).ToUniversalTime() : v.ToUniversalTime(),
            convertFromProviderExpression: v => DateTime.SpecifyKind(v, kind),
            mappingHints)
    {
    }

    /// <summary>
    /// Конвертер для DateTime в формате UTC.
    /// </summary>
    public static readonly DateTimeKindValueConverter Utc = new(DateTimeKind.Utc);

    /// <summary>
    /// Конвертер для DateTime в локальном формате.
    /// </summary>
    public static readonly DateTimeKindValueConverter Local = new(DateTimeKind.Local);

    /// <summary>
    /// Конвертер для DateTime с неопределённым форматом.
    /// </summary>
    public static readonly DateTimeKindValueConverter Unspecified = new(DateTimeKind.Unspecified);
}