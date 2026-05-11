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
        private readonly List<string> _errorLog = new List<string>();

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
                    try
                    {
                        var validationResult = validator.validate(fieldDefinition, formSubmission.Data);
                        if (!validationResult.IsValid)
                        {
                            result = validationResult;
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
                    catch (Exception e) //If missing field or any other error occurs during validation, log the error and continue with the next validator
                    {
                        _errorLog.Add(e.Message);

                        continue;
                    }

                }
                results.Add(result);

            }

            return new FieldResults(success, _errorLog, results);
        }


    }
}
