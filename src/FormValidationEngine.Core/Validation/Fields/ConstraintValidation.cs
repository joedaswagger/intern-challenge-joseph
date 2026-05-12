using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class ConstraintValidation : IFieldValidation
    {
        public bool CanValidate(FieldDefinition fieldDefinition)
        {
            return true; // All can validate, assuming that all fields can have constraints
        }

        public FieldValidationResult Validate(FieldDefinition fieldDefinition, Dictionary<string, string> data)
        {
            if(fieldDefinition.Constraints != null)
            {

                //MinLength/MaxLength checks
                if (fieldDefinition.Constraints.MaxLength.HasValue && data[fieldDefinition.Id].Length > fieldDefinition.Constraints.MaxLength.Value)
                {
                    return ResultFactory.Invalid(fieldDefinition, $"Value length ({data[fieldDefinition.Id].Length}) is less than minimum length ({fieldDefinition.Constraints.MaxLength})");
                   
                }
                if (fieldDefinition.Constraints.MinLength.HasValue && data[fieldDefinition.Id].Length < fieldDefinition.Constraints.MinLength.Value)
                {
                    return ResultFactory.Invalid(fieldDefinition, $"Value length ({data[fieldDefinition.Id].Length}) is more than maximum length ({fieldDefinition.Constraints.MinLength})");
                }

                //Min/Max checks
                if(fieldDefinition.Constraints.Min.HasValue && Int32.Parse(data[fieldDefinition.Id]) < fieldDefinition.Constraints.Min)
                {
                    return ResultFactory.Invalid(fieldDefinition, $"Value {Int32.Parse(data[fieldDefinition.Id])} falls below minimum allowed value of {fieldDefinition.Constraints.Min}");
                }

                if (fieldDefinition.Constraints.Max.HasValue && Int32.Parse(data[fieldDefinition.Id]) > fieldDefinition.Constraints.Max)
                {
                    return ResultFactory.Invalid(fieldDefinition, $"Value {Int32.Parse(data[fieldDefinition.Id])} exceeds maximum allowed value of {fieldDefinition.Constraints.Max}");
                }

                //Pattern check

                if (!string.IsNullOrEmpty(fieldDefinition.Constraints.Pattern) && !System.Text.RegularExpressions.Regex.IsMatch(data[fieldDefinition.Id], fieldDefinition.Constraints.Pattern)) //If pattern req exists and regex does not match
                {
                     return ResultFactory.Invalid(fieldDefinition, $"Value does not match required pattern: {fieldDefinition.Constraints.Pattern}");
                }

                //AllowedValues check

                if(fieldDefinition.Constraints.AllowedValues != null && fieldDefinition.Constraints.AllowedValues.Count > 0 && !fieldDefinition.Constraints.AllowedValues.Contains(data[fieldDefinition.Id]))
                {
                    return ResultFactory.Invalid(fieldDefinition, $"Value is not in the list of allowed values: {string.Join(", ", fieldDefinition.Constraints.AllowedValues)}");
                    
                }

            }
            return ResultFactory.Valid(fieldDefinition, "Valid"); //Default (valid or N/A)
        }
    }
}
