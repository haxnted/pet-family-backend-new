namespace PetFamily.SharedKernel.Tests.Abstractions;

/// <summary>
/// Базовый интерфейс для Test Data Builder pattern.
/// </summary>
/// <typeparam name="T">Тип создаваемого объекта.</typeparam>
public interface IBuilder<out T>
{
	/// <summary>
	/// Создает и возвращает сконфигурированный объект.
	/// </summary>
	T Build();
}