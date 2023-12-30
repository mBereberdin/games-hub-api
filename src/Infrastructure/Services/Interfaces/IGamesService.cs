namespace Infrastructure.Services.Interfaces;

using Domain.Models;

/// <summary>
/// Сервис игр.
/// </summary>
public interface IGamesService
{
    /// <summary>
    /// Зарегистрировать игру.
    /// </summary>
    /// <param name="game">Игра.</param>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    public Task RegisterGameAsync(Game game,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получить зарегистрированные игры.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    /// <returns>Перечислиение зарегистрированных игр.</returns>
    public Task<IEnumerable<Game>> GetRegisteredGamesAsync(
        CancellationToken cancellationToken);
}