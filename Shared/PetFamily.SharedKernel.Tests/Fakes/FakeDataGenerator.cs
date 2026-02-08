using Bogus;

namespace PetFamily.SharedKernel.Tests.Fakes;

/// <summary>
/// Базовый генератор фейковых данных с использованием Bogus.
/// </summary>
public static class FakeDataGenerator
{
    private static readonly Faker Faker = new("ru");

    /// <summary>
    /// Генерирует новый уникальный идентификатор GUID.
    /// </summary>
    /// <returns>Новый GUID.</returns>
    public static Guid Guid() => System.Guid.NewGuid();

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
    /// Генерирует случайный email адрес.
    /// </summary>
    /// <returns>Email адрес.</returns>
    public static string Email() => Faker.Internet.Email();

    /// <summary>
    /// Генерирует случайный номер телефона в формате +7##########.
    /// </summary>
    /// <returns>Номер телефона.</returns>
    public static string PhoneNumber() => Faker.Phone.PhoneNumber("+7##########");

    /// <summary>
    /// Генерирует случайное описание из заданного количества слов.
    /// </summary>
    /// <param name="minWords">Минимальное количество слов.</param>
    /// <param name="maxWords">Максимальное количество слов.</param>
    /// <returns>Случайное описание.</returns>
    public static string Description(int minWords = 5, int maxWords = 20)
        => Faker.Lorem.Sentence(Faker.Random.Int(minWords, maxWords));

    /// <summary>
    /// Генерирует случайный текст заданной длины.
    /// </summary>
    /// <param name="length">Длина текста.</param>
    /// <returns>Случайный текст.</returns>
    public static string Text(int length) => Faker.Lorem.Letter(length);

    /// <summary>
    /// Генерирует случайное число в заданном диапазоне.
    /// </summary>
    /// <param name="min">Минимальное значение.</param>
    /// <param name="max">Максимальное значение.</param>
    /// <returns>Случайное число.</returns>
    public static int Number(int min = 0, int max = 100) => Faker.Random.Int(min, max);

    /// <summary>
    /// Генерирует случайную дату в прошлом.
    /// </summary>
    /// <param name="yearsBack">Количество лет назад.</param>
    /// <returns>Дата в прошлом.</returns>
    public static DateTime PastDate(int yearsBack = 5) => Faker.Date.Past(yearsBack);

    /// <summary>
    /// Генерирует случайную дату в будущем.
    /// </summary>
    /// <param name="yearsForward">Количество лет вперёд.</param>
    /// <returns>Дата в будущем.</returns>
    public static DateTime FutureDate(int yearsForward = 1) => Faker.Date.Future(yearsForward);

    /// <summary>
    /// Генерирует случайный адрес (город, улица, номер здания).
    /// </summary>
    /// <returns>Кортеж с компонентами адреса.</returns>
    public static (string City, string Street, string Building) Address()
        => (Faker.Address.City(), Faker.Address.StreetName(), Faker.Address.BuildingNumber());

    /// <summary>
    /// Генерирует случайное имя питомца.
    /// </summary>
    /// <returns>Имя питомца.</returns>
    public static string PetName() => Faker.Name.FirstName();

    /// <summary>
    /// Генерирует случайный вид животного из предопределённого списка.
    /// </summary>
    /// <returns>Вид животного.</returns>
    public static string AnimalKind() => Faker.PickRandom("Собака", "Кошка", "Птица", "Грызун", "Рептилия");

    /// <summary>
    /// Генерирует случайное название породы.
    /// </summary>
    /// <returns>Название породы.</returns>
    public static string BreedName() => Faker.Lorem.Word() + " " + Faker.Lorem.Word();

    /// <summary>
    /// Выбирает случайный элемент из коллекции.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="items">Коллекция элементов.</param>
    /// <returns>Случайный элемент.</returns>
    public static T PickRandom<T>(IEnumerable<T> items) => Faker.PickRandom(items);

    /// <summary>
    /// Генерирует список элементов заданного размера с помощью указанной фабрики.
    /// </summary>
    /// <typeparam name="T">Тип элементов.</typeparam>
    /// <param name="generator">Функция-генератор элементов.</param>
    /// <param name="count">Количество элементов для генерации.</param>
    /// <returns>Список сгенерированных элементов.</returns>
    public static List<T> GenerateMany<T>(Func<T> generator, int count)
        => Enumerable.Range(0, count).Select(_ => generator()).ToList();

    /// <summary>
    /// Генерирует случайное булево значение с заданной вероятностью.
    /// </summary>
    /// <param name="probability">Вероятность получения true (от 0.0 до 1.0).</param>
    /// <returns>Случайное булево значение.</returns>
    public static bool Bool(float probability = 0.5f) => Faker.Random.Bool(probability);
}
