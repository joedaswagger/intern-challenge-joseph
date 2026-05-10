using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class ConstraintValidation : IFieldValidation
    {
        public FieldValidationResult validate(FieldDefinition fieldDefinition, Dictionary<string, string> data)
        {

            return ResultFactory.Valid(fieldDefinition); //Default (valid)
        }
    }
}
