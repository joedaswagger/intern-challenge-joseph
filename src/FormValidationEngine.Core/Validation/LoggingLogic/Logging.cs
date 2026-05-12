using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;


namespace FormValidationEngine.Core.Validation.Logging
{
    public class Logging : ILogging
    {

        private readonly ILogger<Logging> _logger;

        public Logging()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            var logger = loggerFactory.CreateLogger<Logging>();
        }

        public Logging(ILogger<Logging> logger)
        {
            _logger = logger;
        }
        public void Error(string message)
        {
            _logger.LogError("{Message}", message);
        }

        public void Error(Exception exception, string message)
        {
            _logger.LogError(exception, "{Message}", message);
        }

        public void Info(string message)
        {
            _logger.LogInformation("{Message}", message);
        }

        public void Warning(string message)
        {
            _logger.LogWarning("{Message}", message);
        }
    }
}
