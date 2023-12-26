namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Тестовый контроллер.
/// </summary>
[Route("[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<TestsController> _logger;

    /// <inheritdoc />
    /// <param name="logger">Логгер.</param>
    public TestsController(ILogger<TestsController> logger)
    {
        _logger = logger;
        _logger.LogDebug($"Инициализирован {nameof(TestsController)}.");
    }

    /// <summary>
    /// Получить тестовую строку.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены выполнения операции.</param>>
    /// <returns>Тестовая строка в формате JSON.</returns>
    [HttpGet("string")]
    public async Task<OkObjectResult> GetTestString(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        _logger.LogInformation("Поступил запрос на получение тестовой строки.");

        var testString = await Task.Run(() => "Тест", cancellationToken);
        
        _logger.LogInformation("Запрос получения тестовой строки был обработан.");
        _logger.LogDebug($"Полученная тестовая строка: {testString}.");

        return Ok(testString);
    }
}