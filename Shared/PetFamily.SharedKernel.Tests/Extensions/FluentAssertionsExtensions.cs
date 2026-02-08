using FluentAssertions.Primitives;

namespace PetFamily.SharedKernel.Tests.Extensions;

/// <summary>
/// Расширения для FluentAssertions.
/// </summary>
public static class FluentAssertionsExtensions
{
    /// <summary>
    /// Проверяет, что строка является валидным GUID.
    /// </summary>
    /// <param name="assertions">Утверждение строки.</param>
    /// <param name="because">Причина проверки (опционально).</param>
    /// <param name="becauseArgs">Аргументы для форматирования причины.</param>
    /// <returns>Ограничение для цепочки утверждений.</returns>
    public static AndConstraint<StringAssertions> BeValidGuid(
        this StringAssertions assertions,
        string because = "",
        params object[] becauseArgs)
    {
        var isValid = Guid.TryParse(assertions.Subject, out _);
        isValid.Should().BeTrue(because, becauseArgs);
        return new AndConstraint<StringAssertions>(assertions);
    }

    /// <summary>
    /// Проверяет, что DateTimeOffset близок к текущему времени в пределах заданной точности.
    /// </summary>
    /// <param name="assertions">Утверждение DateTimeOffset.</param>
    /// <param name="precision">Допустимое отклонение от текущего времени.</param>
    /// <param name="because">Причина проверки (опционально).</param>
    /// <param name="becauseArgs">Аргументы для форматирования причины.</param>
    /// <returns>Ограничение для цепочки утверждений.</returns>
    public static AndConstraint<DateTimeOffsetAssertions> BeCloseToNow(
        this DateTimeOffsetAssertions assertions,
        TimeSpan precision,
        string because = "",
        params object[] becauseArgs)
    {
        assertions.Subject.Should().BeCloseTo(DateTimeOffset.UtcNow, precision, because, becauseArgs);
        return new AndConstraint<DateTimeOffsetAssertions>(assertions);
    }

    /// <summary>
    /// Проверяет, что DateTime близок к текущему UTC времени в пределах заданной точности.
    /// </summary>
    /// <param name="assertions">Утверждение DateTime.</param>
    /// <param name="precision">Допустимое отклонение от текущего UTC времени.</param>
    /// <param name="because">Причина проверки (опционально).</param>
    /// <param name="becauseArgs">Аргументы для форматирования причины.</param>
    /// <returns>Ограничение для цепочки утверждений.</returns>
    public static AndConstraint<DateTimeAssertions> BeCloseToUtcNow(
        this DateTimeAssertions assertions,
        TimeSpan precision,
        string because = "",
        params object[] becauseArgs)
    {
        assertions.Subject.Should().BeCloseTo(DateTime.UtcNow, precision, because, becauseArgs);
        return new AndConstraint<DateTimeAssertions>(assertions);
    }
}
