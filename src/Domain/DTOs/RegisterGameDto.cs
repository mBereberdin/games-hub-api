namespace Domain.DTOs;

/// <summary>
/// Дто регистрации игры.
/// </summary>
public record RegisterGameDto
{
    /// <inheritdoc cref="RegisterGameDto"/>
    /// <param name="name">Наименование игры.</param>
    /// <param name="previewPicture">Изображение-предпросмотр игры.</param>
    public RegisterGameDto(string name, string? previewPicture)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Наименование игры не должно быть пустым или состоять только из пробелов.");
        }

        Name = name;
        PreviewPicture = previewPicture;
    }

    /// <summary>
    /// Наименование игры.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Изображение-предпросмотр игры.
    /// </summary>
    public string? PreviewPicture { get; init; }
}