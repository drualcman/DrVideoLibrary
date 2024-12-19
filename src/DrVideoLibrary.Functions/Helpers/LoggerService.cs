namespace DrVideoLibrary.Functions.Helpers;

internal class LoggerService : ILogger
{
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        // Puedes implementar un comportamiento para gestionar contextos si lo necesitas,
        // pero para ahora devolvemos un Disposable vacío.
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        // Indica qué niveles de logging están habilitados. En este caso habilitamos todos.
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        // Usa el formatter para construir el mensaje de logging.
        var message = formatter(state, exception);

        // Puedes enviar esto a un destino de logging (consola, archivo, etc.).
        Console.WriteLine($"[{DateTime.Now}] [{logLevel}] {message}");

        if (exception != null)
        {
            Console.WriteLine($"Exception: {exception.Message}");
        }
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new NullScope();
        public void Dispose() { }
    }
}
internal class LoggerService<TModel> : ILogger<TModel>
{
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        // Puedes implementar un comportamiento para gestionar contextos si lo necesitas,
        // pero para ahora devolvemos un Disposable vacío.
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        // Indica qué niveles de logging están habilitados. En este caso habilitamos todos.
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        // Usa el formatter para construir el mensaje de logging.
        var message = formatter(state, exception);

        // Puedes enviar esto a un destino de logging (consola, archivo, etc.).
        Console.WriteLine($"[{DateTime.Now}] [{logLevel}] {message}");

        if (exception != null)
        {
            Console.WriteLine($"Exception: {exception.Message}");
        }
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new NullScope();
        public void Dispose() { }
    }
}
