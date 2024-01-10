namespace Domain.DTOs;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Дто регистрации игры.
/// </summary>
/// <param name="Name">Наименование игры.</param>
/// <param name="PreviewPicture">Изображение-предпросмотр игры.</param>
public record RegisterGameDto(string Name, string? PreviewPicture)
{
    /// <summary>
    /// Наименование игры.
    /// </summary>
    [Required(AllowEmptyStrings = false,
        ErrorMessage =
            "Наименование игры не должно быть пустым или состоять только из пробелов.")]
    public required string Name { get; init; } = Name;

    /// <summary>
    /// Изображение-предпросмотр игры.
    /// </summary>
    public string? PreviewPicture { get; init; } = PreviewPicture;
}