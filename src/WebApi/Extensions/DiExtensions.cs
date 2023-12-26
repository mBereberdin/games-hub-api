namespace WebApi.Extensions;

using Serilog;

using WebApi.Middlewares;

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
}