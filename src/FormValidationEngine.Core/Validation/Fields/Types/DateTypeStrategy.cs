using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields.Types
{
    public class DateTypeStrategy : ITypeStrategy
    {
        public FieldType Type => FieldType.Date;

        public FieldValidationResult ValidateType(FieldDefinition fieldDefinition, string value)
        {
            DateTime date;
            if (fieldDefinition.Type == FieldType.Date && !DateTime.TryParse(value, out date))
            {
                return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be a valid date.");

            }
            return ResultFactory.Valid(fieldDefinition, "Valid");
        }

      
    }
}
