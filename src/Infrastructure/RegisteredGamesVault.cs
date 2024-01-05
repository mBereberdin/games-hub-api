namespace Infrastructure;

using Domain.Models;

using Microsoft.Extensions.Logging;

/// <summary>
/// Хранилище зарегистрированных игр.
/// </summary>
public class RegisteredGamesVault
{
    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<RegisteredGamesVault> _logger;

    /// <summary>
    /// Зарегистрированные игры.
    /// </summary>
    private readonly ISet<Game> _registeredGames;

    /// <summary>
    /// Таймеры удаления игр.
    /// </summary>
    /// <remarks>Ключ - наименование игры; значение - источник токена отмены удаления игры.</remarks>
    private readonly IDictionary<string, CancellationTokenSource>
        _removeGamesTimers;

    /// <inheritdoc cref="RegisteredGamesVault"/>
    /// <param name="logger">Логгер.</param>
    public RegisteredGamesVault(ILogger<RegisteredGamesVault> logger)
    {
        _logger = logger;

        _logger.LogDebug($"Инициализация: {nameof(RegisteredGamesVault)}");

        _registeredGames = new HashSet<Game>();
        _removeGamesTimers = new Dictionary<string, CancellationTokenSource>();

        _logger.LogDebug(
            $"{nameof(RegisteredGamesVault)} - успешно инициализирован.");
    }

    /// <summary>
    /// Таймеры на удаление игр.
    /// </summary>
    public IDictionary<string, CancellationTokenSource> RemoveGamesTimers
    {
        get
        {
            _logger.LogInformation(
                "Получение таймеров на удаление игр.");
            _logger.LogDebug(
                "Таймеры на удаление игр: {removeGamesTimers}.",
                _removeGamesTimers);

            return _removeGamesTimers;
        }
    }

    /// <summary>
    /// Зарегистрированные игры.
    /// </summary>
    public IEnumerable<Game> RegisteredGames
    {
        get
        {
            _logger.LogInformation(
                "Получение зарегистрированных игр из хранилища.");
            _logger.LogDebug(
                "Зарегистрированные игры хранилища: {registeredGames}.",
                _registeredGames);

            return _registeredGames;
        }
    }

    /// <summary>
    /// Зарегистрировать игру.
    /// </summary>
    /// <param name="game">Игра.</param>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    public async Task RegisterGameAsync(Game game,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Регистрация игры в хранилище.");
        _logger.LogDebug("Игра: {game}.", game);

        await Task.Run(() =>
        {
            if (!_registeredGames.Add(game))
            {
                _logger.LogWarning(
                    "Не удалось добавить игру в хранилище. Скорее всего данный элемент уже пресутствует в коллекции.");
            }
        }, cancellationToken);

        _logger.LogInformation(
            "Регистрация игры в хранилище - успешно завершено.");
    }

    /// <summary>
    /// Удалить игру.
    /// </summary>
    /// <param name="gameName">Наименование игры.</param>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    public async Task RemoveGameAsync(string gameName,
        CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation(
                "Удаление игры по наименованию из хранилища.");
            _logger.LogDebug("Наименование игры: {gameName}.", gameName);

            RemoveGamesTimers.Remove(gameName);

            var gameToRemove = _registeredGames.First(registeredGame =>
                registeredGame.Name.Equals(gameName,
                    StringComparison.OrdinalIgnoreCase));

            _registeredGames.Remove(gameToRemove);

            _logger.LogInformation(
                "Удаление игры по наименованию из хранилища - успешно завершено.");
            _logger.LogDebug("Удаленная игра: {deletedGame}", gameToRemove);
        }, cancellationToken);
    }
}