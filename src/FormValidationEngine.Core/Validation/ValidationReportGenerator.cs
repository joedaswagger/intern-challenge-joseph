using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Dependencies;
using FormValidationEngine.Core.Validation.Fields;
using FormValidationEngine.Core.Validation.Fields.Types;
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
            DependencyResolutionOrchestrator resolver = new DependencyResolutionOrchestrator();

            DependencyResults dependencyResults = resolver.Resolve(formDefinition.Fields);
            
            if(!dependencyResults.success) // if dependency resolution fails, don't validate fields, make failed report immediately
            {
                var failReport = new ValidationReport
                {
                    SubmissionId = formSubmission.SubmissionId,
                    FormId = formDefinition.FormId,
                    IsValid = dependencyResults.success, //Both have to succeed for a valid form
                    ExecutionOrder = dependencyResults.executionOrder,
                    Errors = dependencyResults.errors, 
                };

                return failReport;
            }

            var validators = new List<IFieldValidation> //Initialize all validators, add new ones here when necessary (respects OCP)
            { new RequiredFieldValidation(),
                new TypeValidation(new List<ITypeStrategy> {new TextTypeStrategy(), new NumberTypeStrategy(), new DateTypeStrategy(), new ChoiceTypeStrategy(), new BooleanTypeStrategy(), }), //Initialize the type validation with all type strategies, since we want it to be able to validate all types. You can add a new strategy here every time you want to add something new (respects OCP)
                new ConstraintValidation(),
                new CalculatedFieldValidation(),
                new CrossFieldValidation(formSubmission.SubmittedAt),
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
