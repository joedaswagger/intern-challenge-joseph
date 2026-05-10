using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public interface IFieldValidation
    {
        FieldValidationResult validate(FieldDefinition fieldDefinition, Dictionary<string, string> data);

    }
}
