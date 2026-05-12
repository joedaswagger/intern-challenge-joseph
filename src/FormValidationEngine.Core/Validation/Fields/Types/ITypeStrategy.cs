using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields.Types
{
    public interface ITypeStrategy
    {
        FieldType Type { get; }

        FieldValidationResult ValidateType(FieldDefinition fieldDefinition, string value); //Individual validator
    }
}
