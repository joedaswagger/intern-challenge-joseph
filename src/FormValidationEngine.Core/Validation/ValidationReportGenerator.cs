using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation
{
    public class ValidationReportGenerator
    {
        public ValidationReport generate(FormDefinition formDefinition, FormSubmission formSubmission)
        {
            DependencyResolver resolver = new DependencyResolver();

            DependencyResults dependencyResults = resolver.Resolve(formDefinition.Fields);
            

            var report = new ValidationReport
            {
                SubmissionId = formSubmission.SubmissionId,
                FormId = formDefinition.FormId,
                IsValid = dependencyResults.success,
                ExecutionOrder = dependencyResults.executionOrder,
                Errors = dependencyResults.errors,
            };
            
            
            return report;
        }
    }
}
