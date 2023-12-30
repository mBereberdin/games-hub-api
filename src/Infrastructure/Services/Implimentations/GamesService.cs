namespace Infrastructure.Services.Implimentations;

using Domain.Models;

using Infrastructure.Services.Interfaces;

using Microsoft.Extensions.Logging;

/// <inheritdoc />
public class GamesService : IGamesService
{
    /// <inheritdoc cref="RegisteredGamesVault"/>
    private readonly RegisteredGamesVault _registeredGamesVault;

    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<IGamesService> _logger;

    /// <inheritdoc cref="IGamesService"/>
    /// <param name="registeredGamesVault">Хранилище зарегистрированных игр.</param>
    /// <param name="logger">Логгер.</param>
    public GamesService(RegisteredGamesVault registeredGamesVault,
        ILogger<IGamesService> logger)
    {
        _logger = logger;
        _logger.LogDebug($"Инициализация: {nameof(IGamesService)}.");

        _registeredGamesVault = registeredGamesVault;

        _logger.LogDebug($"Инициализирован: {nameof(IGamesService)}.");
    }

    /// <inheritdoc />
    public async Task RegisterGameAsync(Game game,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Регистрация игры.");
        _logger.LogDebug($"Игра: {game}");

        await _registeredGamesVault.RegisterGameAsync(game, cancellationToken);

        _logger.LogInformation("Игра успешно зарегистрирована.");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetRegisteredGamesAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Получение зарегистрированных игр.");

        var registeredGames = await Task.Run(
            () => _registeredGamesVault.RegisteredGames, cancellationToken);

        _logger.LogInformation("Зарегистрированные игры получены успешно.");
        _logger.LogDebug($"Зарегистрированные игры: {registeredGames}.");

        return registeredGames;
    }
}