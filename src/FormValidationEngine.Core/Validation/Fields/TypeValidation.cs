using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Fields.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class TypeValidation : IFieldValidation
    {
        private readonly Dictionary<FieldType, ITypeStrategy> _typesToValidate;

        public TypeValidation(IEnumerable<ITypeStrategy> typesToValidate)
        {
            _typesToValidate = typesToValidate.ToDictionary(t => t.Type);
        }

        public bool CanValidate(FieldDefinition fieldDefinition)
        {
            return fieldDefinition.Type != FieldType.Calculated;
        }

        public FieldValidationResult Validate(
            FieldDefinition fieldDefinition,
            Dictionary<string, string> data)
        {
            if (!data.TryGetValue(fieldDefinition.Id, out var value))
            {
                return ResultFactory.Valid(fieldDefinition, "Valid");
            }

            if (!_typesToValidate.TryGetValue(fieldDefinition.Type, out var strategy))
            {
                return ResultFactory.Valid(fieldDefinition, "Valid");
            }

            return strategy.ValidateType(fieldDefinition, value);
        }
    }
}
