namespace Domain.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Игра.
/// </summary>
public class Game
{
    /// <summary>
    /// Наименование игры.
    /// </summary>
    [Required(AllowEmptyStrings = false,
        ErrorMessage =
            "Наименование игры не должно быть пустым или состоять только из пробелов.")]
    public required string Name { get; set; }

    /// <summary>
    /// Изображение-предпросмотр игры.
    /// </summary>
    public string? PreviewPicture { get; set; }
}