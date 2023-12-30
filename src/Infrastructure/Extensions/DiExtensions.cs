namespace Infrastructure.Extensions;

using Infrastructure.Middlewares;
using Infrastructure.Services.Implimentations;
using Infrastructure.Services.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

/// <summary>
/// Расширения для DI.
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Добавить промежуточные слои приложения.
    /// </summary>
    /// <param name="builder">Строитель приложения.</param>
    public static void AddAppMiddlewares(this IApplicationBuilder builder)
    {
        Log.Logger.Information("Добавление промежуточных слоев приложения.");

        builder.UseMiddleware<ExceptionsMiddleware>();

        Log.Logger.Information("Промежуточные слои приложения добавлены.");
    }

    /// <summary>
    /// Добавить сервисы.
    /// </summary>
    /// <param name="services">Коллекция сервисов приложения.</param>
    public static void AddServices(this IServiceCollection services)
    {
        Log.Logger.Information("Добавление сервисов.");

        services.AddTransient<IGamesService, GamesService>();
        services.AddSingleton<RegisteredGamesVault>();

        Log.Logger.Information("Сервисы добавлены.");
    }
}