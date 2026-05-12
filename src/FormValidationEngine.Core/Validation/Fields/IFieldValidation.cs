using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public interface IFieldValidation
    {
        FieldValidationResult Validate(FieldDefinition fieldDefinition, Dictionary<string, string> data); //Master validation method
        bool CanValidate(FieldDefinition fieldDefinition); // Method to check if the validation is applicable for the given field definition
    }
}
