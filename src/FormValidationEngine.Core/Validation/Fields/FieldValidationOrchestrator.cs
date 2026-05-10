using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Dependencies;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class FieldValidationOrchestrator

    {
        private List<FieldValidationResult> results = new List<FieldValidationResult>();
        private readonly IEnumerable<IFieldValidation> _validators;

        public FieldValidationOrchestrator(IEnumerable<IFieldValidation> validators)
        {
            _validators = validators;
        }


        public FieldResults ValidateFields(List<FieldDefinition> fieldDefinitions, FormSubmission formSubmission, List<string> executionOrder)
        {
            var sortedList = fieldDefinitions.OrderBy(fd => executionOrder.IndexOf(fd.Id)).ToList();
            var success = true;
            foreach (var fieldDefinition in sortedList)
            {

                FieldValidationResult result = new FieldValidationResult();

                foreach (var validator in _validators)
                {

                    var validationResult = validator.validate(fieldDefinition, formSubmission.Data);
                    if (!validationResult.IsValid)
                    {
                        result = validationResult;
                        success = false;
                        break; // Break if one validator fails
                    }
                    else
                    {
                        result = validationResult;
                        if (result.Severity == ValidationSeverity.Warning) // Break if it's a warning
                        {
                            break;
                        }
                    }
                    


                }
                results.Add(result);
                
            }
            FieldResults fieldAnswers = new FieldResults(success, new List<string>(), results);
            return fieldAnswers;
        }

        
    }
}
