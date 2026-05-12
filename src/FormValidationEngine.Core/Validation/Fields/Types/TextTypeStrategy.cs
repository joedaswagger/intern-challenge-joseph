using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields.Types
{
    public class TextTypeStrategy : ITypeStrategy
    {
        public FieldType Type => FieldType.Text;

        public FieldValidationResult ValidateType(FieldDefinition fieldDefinition, string value)
        {
            if (fieldDefinition.Type == FieldType.Text && string.IsNullOrWhiteSpace(value))
            {
                return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} cannot be empty.");

            }
            return ResultFactory.Valid(fieldDefinition, "Valid");
        }
    }
}
