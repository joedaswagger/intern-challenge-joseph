using System;
using System.IO;
using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation;
using Newtonsoft.Json;

namespace FormValidationEngine.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

            var generator = new ValidationReportGenerator();

            var report = generator.generate(formDefinition, submission);

            var output = JsonConvert.SerializeObject(report, Formatting.Indented);
            Console.WriteLine(output);

            Environment.ExitCode = report.IsValid ? 0 : 1;
        }
    }
}
