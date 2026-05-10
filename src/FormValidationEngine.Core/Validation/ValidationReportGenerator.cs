using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Dependencies;
using FormValidationEngine.Core.Validation.Fields;
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

            var validators = new List<IFieldValidation>
            { new RequiredFieldValidation(),
                new TypeValidation(),
            };
            FieldValidationOrchestrator fieldValidation = new FieldValidationOrchestrator(validators);

            FieldResults fieldResults = fieldValidation.ValidateFields(formDefinition.Fields,formSubmission, dependencyResults.executionOrder);
            

            var report = new ValidationReport
            {
                SubmissionId = formSubmission.SubmissionId,
                FormId = formDefinition.FormId,
                IsValid = dependencyResults.success && fieldResults.success,
                ExecutionOrder = dependencyResults.executionOrder,
                Results = fieldResults.fields,
                Errors = dependencyResults.errors,
            };
            
            
            return report;
        }
    }
}
