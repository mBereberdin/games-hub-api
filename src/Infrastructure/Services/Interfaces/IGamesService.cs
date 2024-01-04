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

    /// <summary>
    /// Удалить игру по таймеру.
    /// </summary>
    /// <param name="gameName">Наименование игры.</param>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    public void RemoveGameWithTimer(string gameName,
        CancellationToken cancellationToken);

    /// <summary>
    /// Зарегистрирована ли игра.
    /// </summary>
    /// <param name="gameName">Название игры.</param>
    /// <param name="game">Игра, если она зарегистрирована.</param>
    /// <returns>True -  если игра зарегистрирована, иначе - false.</returns>
    public bool IsGameRegistered(string gameName, out Game? game);

    /// <summary>
    /// Заменить таймер удаления игры.
    /// </summary>
    /// <param name="game">Игра.</param>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    /// <returns>Задачу.</returns>
    public Task ReplaceGameTimerAsync(Game game,
        CancellationToken cancellationToken);
}