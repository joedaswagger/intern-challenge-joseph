using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Dependencies;
using FormValidationEngine.Core.Validation.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new ConstraintValidation(),
                new CalculatedFieldValidation(),
            };
            FieldValidationOrchestrator fieldValidation = new FieldValidationOrchestrator(validators);

            FieldResults fieldResults = fieldValidation.ValidateFields(formDefinition.Fields,formSubmission, dependencyResults.executionOrder);
            

            var report = new ValidationReport
            {
                SubmissionId = formSubmission.SubmissionId,
                FormId = formDefinition.FormId,
                IsValid = dependencyResults.success && fieldResults.success, //Both have to succeed for a valid form
                ExecutionOrder = dependencyResults.executionOrder,
                Results = fieldResults.fields,
                Errors = dependencyResults.errors.Any() ? dependencyResults.errors : fieldResults.errors, //If there are dependency errors, put them first since they're breaking the system first, otherwise put field errors
            };
            
            
            return report;
        }
    }
}
