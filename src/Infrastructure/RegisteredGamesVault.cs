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
    private Game[] _registeredGames;

    /// <inheritdoc cref="RegisteredGamesVault"/>
    /// <param name="logger">Логгер.</param>
    public RegisteredGamesVault(ILogger<RegisteredGamesVault> logger)
    {
        _logger = logger;

        _logger.LogDebug($"Инициализация: {nameof(RegisteredGamesVault)}");

        _registeredGames = Array.Empty<Game>();

        _logger.LogDebug(
            $"{nameof(RegisteredGamesVault)} - успешно инициализирован.");
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
                $"Зарегистрированные игры хранилища: {_registeredGames}.");

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
        _logger.LogDebug($"Игра: {game}.");

        await Task.Run(() =>
        {
            _registeredGames =
                _registeredGames.Append(game).ToArray();
        }, cancellationToken);

        _logger.LogInformation(
            "Регистрация игры в хранилище - успешно завершено.");
    }
}