using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields.Types
{
    public class NumberTypeStrategy : ITypeStrategy
    {
        public FieldType Type => FieldType.Number;

        public FieldValidationResult ValidateType(FieldDefinition fieldDefinition, string value)
        {
            double number;
            if (fieldDefinition.Type == FieldType.Number && !double.TryParse(value, out number))
            {
                return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be a valid number.");

            }
            return ResultFactory.Valid(fieldDefinition, "Valid");
        }
    }
}
