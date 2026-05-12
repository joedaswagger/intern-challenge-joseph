using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields.Types
{
    public class BooleanTypeStrategy : ITypeStrategy
    {
        public FieldType Type => FieldType.Boolean;

        public FieldValidationResult ValidateType(FieldDefinition fieldDefinition, string value)
        {
            bool boolean;
            if (fieldDefinition.Type == FieldType.Boolean && !bool.TryParse(value, out boolean))
            {
                return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be a valid boolean (true/false).");
            }
            return ResultFactory.Valid(fieldDefinition, "Valid");
        }
    }
}
