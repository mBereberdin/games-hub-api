namespace WebApi.Controllers;

using Domain.DTOs;
using Domain.Models;

using Infrastructure.Services.Interfaces;

using Mapster;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер игр.
/// </summary>
[Route("[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<GamesController> _logger;

    /// <inheritdoc />
    private readonly IGamesService _gamesService;

    /// <inheritdoc />
    /// <param name="logger">Логгер.</param>
    /// <param name="gamesService">Сервис игр.</param>
    public GamesController(ILogger<GamesController> logger,
        IGamesService gamesService)
    {
        _logger = logger;
        _logger.LogDebug($"Инициализация: {nameof(GamesController)}.");

        _gamesService = gamesService;

        _logger.LogDebug($"Инициализирован: {nameof(GamesController)}.");
    }

    /// <summary>
    /// Получить зарегистрированные игры.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    /// <returns>Перечисление зарегистрированных игр в формате JSON.</returns>
    /// <response code="200">Когда зарегистрированные игры получены успешно.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDto>),
        StatusCodes.Status200OK)]
    public async Task<OkObjectResult> GetRegisteredGamesAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Получен запрос на получение зарегистрированных игр.");

        var registeredGames =
            await _gamesService.GetRegisteredGamesAsync(cancellationToken);

        var registeredGamesDtos =
            registeredGames.Adapt<IEnumerable<GameDto>>();

        _logger.LogInformation(
            "Зарегистрированные игры получены успешно.");

        return Ok(registeredGamesDtos);
    }

    /// <summary>
    /// Регистрация игры.
    /// </summary>
    /// <param name="registerGameDto">Дто регистрации игры.</param>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>
    /// <response code="204">Когда регистрация игры прошла успешно.</response>
    [HttpPost]
    public async Task<NoContentResult> RegisterGameAsync(
        [FromBody] RegisterGameDto registerGameDto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Получен запрос на регистрацию игры.");
        _logger.LogDebug($"Запрос регистрации игры: {registerGameDto}.");

        var game = registerGameDto.Adapt<Game>();

        await _gamesService.RegisterGameAsync(game, cancellationToken);

        _logger.LogInformation("Запрос регистрации игры - успешно обработан.");

        return NoContent();
    }
}