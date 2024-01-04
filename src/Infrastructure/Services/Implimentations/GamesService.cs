namespace Infrastructure.Services.Implimentations;

using Domain.Models;
using Domain.Settings;

using Infrastructure.Services.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <inheritdoc />
public class GamesService : IGamesService
{
    /// <inheritdoc cref="RegisteredGamesVault"/>
    private readonly RegisteredGamesVault _registeredGamesVault;

    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<IGamesService> _logger;

    /// <inheritdoc cref="GamesTimerSettings"/>
    private readonly GamesTimerSettings _gamesTimerSettings;

    /// <inheritdoc cref="IGamesService"/>
    /// <param name="registeredGamesVault">Хранилище зарегистрированных игр.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="gamesTimerSettingsOptions">Настройки таймеров игр.</param>
    public GamesService(RegisteredGamesVault registeredGamesVault,
        ILogger<IGamesService> logger,
        IOptions<GamesTimerSettings> gamesTimerSettingsOptions)
    {
        _logger = logger;
        _logger.LogDebug($"Инициализация: {nameof(IGamesService)}.");

        _registeredGamesVault = registeredGamesVault;
        _gamesTimerSettings = gamesTimerSettingsOptions.Value;

        _logger.LogDebug($"Инициализирован: {nameof(IGamesService)}.");
    }

    /// <inheritdoc />
    public async Task RegisterGameAsync(Game game,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Проверка не зарегистрирована ли уже игра.");
        _logger.LogDebug("Игра: {0}", game);

        if (IsGameRegistered(game.Name, out var registeredGame))
        {
            _logger.LogInformation("Данная игра уже зарегистрированна.");
            _logger.LogDebug("Игра: {0}", registeredGame);

            await ReplaceGameTimerAsync(registeredGame!, cancellationToken);

            return;
        }


        _logger.LogInformation("Регистрация игры.");
        _logger.LogDebug("Игра: {game}", game);

        await _registeredGamesVault.RegisterGameAsync(game, cancellationToken);
        RemoveGameWithTimer(game.Name,
            cancellationToken);

        _logger.LogInformation("Игра успешно зарегистрирована.");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetRegisteredGamesAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Получение зарегистрированных игр.");

        var registeredGames = await Task.Run(
            () => _registeredGamesVault.RegisteredGames.ToArray(),
            cancellationToken);

        _logger.LogInformation("Зарегистрированные игры получены успешно.");
        _logger.LogDebug("Зарегистрированные игры: {0}.",
            registeredGames.ToArray<object>());

        return registeredGames;
    }

    /// <inheritdoc />
    public void RemoveGameWithTimer(string gameName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var cancellationTokenSource = new CancellationTokenSource();

        // Не ожидаем, т.к. задача фоновая.
        Task.Run(async () =>
            {
                var deleteTime = DateTime.Now.Second +
                                 _gamesTimerSettings.DeleteAfterTime;
                var removeGameTime = deleteTime >= 60
                    ? deleteTime - 60
                    : deleteTime;

                _logger.LogDebug("Игра: {0} удалится в: {1} сек.", gameName,
                    removeGameTime);

                // TODO: придумать/найти нормальную реализацию выполнения задачи по таймеру с возможностью отмены. Кидать поток в сон - крайняя мера. 
                while (DateTime.Now.Second < removeGameTime)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                await _registeredGamesVault.RemoveGameAsync(gameName,
                    cancellationToken);
            },
            cancellationTokenSource.Token);

        _registeredGamesVault.RemoveGamesTimers.Add(gameName,
            cancellationTokenSource);
    }

    /// <inheritdoc />
    public bool IsGameRegistered(string gameName,
        out Game? game)
    {
        _logger.LogInformation("Проверка зарегистрированна ли игра.");
        _logger.LogDebug("Наименование игры: {gameName}.", gameName);

        game = _registeredGamesVault.RegisteredGames.FirstOrDefault(
            registeredGame => registeredGame.Name.Equals(gameName,
                StringComparison.OrdinalIgnoreCase));

        var isGameRegistered = game is not null;

        _logger.LogInformation(
            "Проверка зарегистрированна ли игра - завершено успешно.");
        _logger.LogDebug(
            "Игра с наименованием: {gameName}. - зарегистрирована: {isGameRegistered}",
            gameName, isGameRegistered);

        return isGameRegistered;
    }

    /// <inheritdoc />
    public async Task ReplaceGameTimerAsync(Game game,
        CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            _logger.LogInformation("Замена таймера удаления игры.");
            _logger.LogDebug("Игра: {game}.", game);

            var registeredGameTimer =
                _registeredGamesVault.RemoveGamesTimers[game.Name];
            registeredGameTimer.Cancel();
            _registeredGamesVault.RemoveGamesTimers.Remove(game.Name);

            RemoveGameWithTimer(game.Name,
                cancellationToken);

            _logger.LogInformation("Таймер удаления игры - заменён.");
        }, cancellationToken);
    }
}