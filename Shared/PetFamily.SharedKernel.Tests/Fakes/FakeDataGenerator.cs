using Bogus;

namespace PetFamily.SharedKernel.Tests.Fakes;

/// <summary>
/// Базовый генератор фейковых данных с использованием Bogus.
/// </summary>
public static class FakeDataGenerator
{
    private static readonly Faker Faker = new("ru");

    /// <summary>
    /// Генерирует случайное имя.
    /// </summary>
    /// <returns>Случайное имя.</returns>
    public static string FirstName() => Faker.Name.FirstName();

    /// <summary>
    /// Генерирует случайную фамилию.
    /// </summary>
    /// <returns>Случайная фамилия.</returns>
    public static string LastName() => Faker.Name.LastName();

    /// <summary>
    /// Генерирует случайное отчество (с вероятностью 80%) или null.
    /// </summary>
    /// <returns>Отчество или null.</returns>
    public static string? Patronymic() => Faker.Random.Bool(0.8f)
        ? Faker.Name.FirstName() + "ович"
        : null;

    /// <summary>
    /// Генерирует случайную дату в прошлом.
    /// </summary>
    /// <param name="yearsBack">Количество лет назад.</param>
    /// <returns>Дата в прошлом.</returns>
    public static DateTime PastDate(int yearsBack = 5) => Faker.Date.Past(yearsBack);

    /// <summary>
    /// Генерирует случайное булево значение с заданной вероятностью.
    /// </summary>
    /// <param name="probability">Вероятность получения true (от 0.0 до 1.0).</param>
    /// <returns>Случайное булево значение.</returns>
    public static bool Bool(float probability = 0.5f) => Faker.Random.Bool(probability);
}