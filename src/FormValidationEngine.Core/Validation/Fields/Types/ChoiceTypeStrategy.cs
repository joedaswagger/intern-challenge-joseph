using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields.Types
{
    public class ChoiceTypeStrategy : ITypeStrategy
    {
        public FieldType Type => FieldType.Choice;

        public FieldValidationResult ValidateType(FieldDefinition fieldDefinition, string value)
        {
            if (fieldDefinition.Type == FieldType.Choice && !(value.ToLower() == "yes" || value.ToLower() == "no"))
            {
                return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Id} must be one of the following choices: {string.Join(", ", fieldDefinition.Constraints.AllowedValues)}.");
            }
            return ResultFactory.Valid(fieldDefinition, "Valid");
        }
    }
}
