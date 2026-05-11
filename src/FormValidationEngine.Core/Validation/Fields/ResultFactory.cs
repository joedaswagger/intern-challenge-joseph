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

        public static FieldValidationResult Valid(FieldDefinition fieldDefinition, string message)
        {
            return new FieldValidationResult
            {
                FieldId = fieldDefinition.Id,
                IsValid = true,
                Severity = ValidationSeverity.Info,
                Message = message
            };
        }

        public static FieldValidationResult Warning(FieldDefinition fieldDefinition, string message)
        {
            return new FieldValidationResult
            {
                FieldId = fieldDefinition.Id,
                IsValid = true,
                Severity = ValidationSeverity.Warning,
                Message = message
            };
        }
    }
}
