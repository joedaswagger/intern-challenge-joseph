using System;
using System.IO;
using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation;
using FormValidationEngine.Core.Validation.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FormValidationEngine.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder => //Configure logging to console with a simple format.
            {
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                }

                );
                builder.SetMinimumLevel(LogLevel.Information);
            });

            var logger = loggerFactory.CreateLogger<Logging>();


            logger.LogInformation("Starting...");

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: FormValidationEngine.App <form-definition.json> <submission.json>");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("  dotnet run -- ../../docs/sample-form.json ../../docs/sample-submission.json");
                return;
            }


            var formJson = File.ReadAllText(args[0]);
            var submissionJson = File.ReadAllText(args[1]);

            var formDefinition = JsonConvert.DeserializeObject<FormDefinition>(formJson);
            var submission = JsonConvert.DeserializeObject<FormSubmission>(submissionJson);

            // TODO: Implement the validation pipeline.
            // Given a form definition and a submission, produce a ValidationReport.
            // The report should be serialized as JSON and written to stdout.
            logger.LogInformation("Form and submission processed");


            var validationLogging = new Logging(logger);
            var generator = new ValidationReportGenerator(validationLogging); //Instance of report generator to be called

            logger.LogInformation("Starting validation report...");
            var report = generator.generate(formDefinition, submission);
            logger.LogInformation("Validation report complete. Loading...");
            var output = JsonConvert.SerializeObject(report, Formatting.Indented);
            Console.WriteLine(output);

            Environment.ExitCode = report.IsValid ? 0 : 1;
        }
    }
}
