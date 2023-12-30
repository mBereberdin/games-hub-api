namespace Domain.Models;

/// <summary>
/// Игра.
/// </summary>
public class Game
{
    /// <inheritdoc cref="Game"/>
    /// <param name="name">Наименование игры.</param>
    /// <param name="previewPicture">Изображение-предпросмотр игры.</param>
    public Game(string name, string? previewPicture)
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
    public string Name { get; set; }

    /// <summary>
    /// Изображение-предпросмотр игры.
    /// </summary>
    public string? PreviewPicture { get; set; }
}