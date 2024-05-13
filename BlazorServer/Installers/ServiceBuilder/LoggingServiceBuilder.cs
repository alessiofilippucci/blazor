using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace BlazorServer.Installers.ServiceBuilder
{
    public static class LoggingServiceBuilder
    {
        public static LoggerConfiguration GetConfig()
        {
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("BlazorServer", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: SystemConsoleTheme.Colored);

            var logPath = $"logs{Path.DirectorySeparatorChar}.txt";
            var logTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            var mininumLevel = LogEventLevel.Information;

            loggerConfig.WriteTo.File(logPath, mininumLevel, logTemplate, rollingInterval: RollingInterval.Day);
            return loggerConfig;
        }

        public static void ConfigureLogger(this IServiceCollection services)
        {
            if (Log.Logger == null)
            {
                Log.Logger = GetConfig().CreateLogger();
            }

            services.AddLogging(s =>
            {
                s.AddSerilog(Log.Logger, true);
            });
        }
    }
}
