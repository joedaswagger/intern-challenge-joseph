using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public static class ResultFactory //Factory method to improve code readability and reduce bloat
    {
        public static FieldValidationResult Invalid(FieldDefinition fieldDefinition, string message)
        {
            return new FieldValidationResult
            {
                FieldId = fieldDefinition.Id,
                IsValid = false,
                Severity = ValidationSeverity.Error,
                Message = message
            };
        }

        public static FieldValidationResult Valid(FieldDefinition fieldDefinition)
        {
            return new FieldValidationResult
            {
                FieldId = fieldDefinition.Id,
                IsValid = true,
                Severity = ValidationSeverity.Error,
                Message = "Valid"
            };
        }
    }
}
