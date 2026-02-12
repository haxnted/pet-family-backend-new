using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FileStorage.Application.Validators;

/// <summary>
/// Валидатор для загружаемых файлов.
/// </summary>
public sealed class FormFileValidator : AbstractValidator<IFormFile>
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/jpg",
        "image/png",
        "image/gif",
        "image/webp",
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    public FormFileValidator()
    {
        RuleFor(file => file)
            .NotNull()
            .WithMessage("Файл не может быть пустым");

        RuleFor(file => file.Length)
            .GreaterThan(0)
            .WithMessage("Файл не может быть пустым")
            .LessThanOrEqualTo(MaxFileSizeBytes)
            .WithMessage($"Размер файла не должен превышать {MaxFileSizeBytes / 1024 / 1024} МБ");

        RuleFor(file => file.ContentType)
            .NotEmpty()
            .WithMessage("Тип содержимого файла не указан")
            .Must(contentType => AllowedContentTypes.Contains(contentType))
            .WithMessage($"Недопустимый тип файла. Разрешённые типы: {string.Join(", ", AllowedContentTypes)}");

        RuleFor(file => file.FileName)
            .NotEmpty()
            .WithMessage("Имя файла не указано")
            .MaximumLength(255)
            .WithMessage("Имя файла не должно превышать 255 символов");
    }
}
