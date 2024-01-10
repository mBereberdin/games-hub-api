namespace Domain.Settings;

/// <summary>
/// Настройки таймеров игр.
/// </summary>
public record GamesTimerSettings
{
    /// <summary>
    /// Удалить игру после.
    /// </summary>
    /// <remarks>Указываются секунды.</remarks>
    public int DeleteAfterTime { get; init; } = 5;
}