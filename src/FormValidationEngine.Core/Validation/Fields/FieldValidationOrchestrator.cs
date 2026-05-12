using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Dependencies;
using FormValidationEngine.Core.Validation.Logging;
using Microsoft.Extensions.Logging;
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
        private readonly List<string> _errorList = new List<string>();
        private readonly ILogging _logger;

        public FieldValidationOrchestrator(IEnumerable<IFieldValidation> validators, ILogging logger)
        {
            _validators = validators;
            _logger = logger;
        }


        public FieldResults ValidateFields(List<FieldDefinition> fieldDefinitions, FormSubmission formSubmission, List<string> executionOrder)
        {
            var sortedList = fieldDefinitions.OrderBy(fd => executionOrder.IndexOf(fd.Id)).ToList();
            var success = true;

            foreach (var fieldDefinition in sortedList)
            {

                FieldValidationResult result = new FieldValidationResult();
                _logger.Info($"Validating field {fieldDefinition.Id} with type {fieldDefinition.Type}...");

                foreach (var validator in _validators)
                {
                    try
                    {
                        if (!validator.CanValidate(fieldDefinition))
                        {
                            continue; // Skip if the validator cannot validate this field
                        }
                        var validationResult = validator.Validate(fieldDefinition, formSubmission.Data);
                        if (!validationResult.IsValid)
                        {   
                            _logger.Error($"Validation failed for field {fieldDefinition.Id} with validator {validator.GetType().Name}: {validationResult.Message}");
                            result = validationResult;
                            success = false;
                            break; // Break if one validator fails
                        }
                        else
                        {
                            result = validationResult;
                            if (result.Severity == ValidationSeverity.Warning) // Break if it's a warning
                            {   
                                _logger.Warning($"Validation warning for field {fieldDefinition.Id} with validator {validator.GetType().Name}: {validationResult.Message}");
                                break;
                            }
                        }
                        _logger.Info($"Validation succeeded for field {fieldDefinition.Id} with validator {validator.GetType().Name}");
                    }
                    catch (Exception e) //If missing field or any other error occurs during validation, log the error and continue with the next validator
                    {
                        _errorList.Add(e.Message);
                        success = false;
                        continue;
                    }

                }
                results.Add(result);

            }

            return new FieldResults(success, _errorList, results);
        }


    }
}
