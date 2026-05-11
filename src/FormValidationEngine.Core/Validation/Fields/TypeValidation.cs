using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class TypeValidation : IFieldValidation
    {

        public FieldValidationResult validate(FieldDefinition fieldDefinition, Dictionary<string, string> data) //Every type but calculated
        {
            /*
             Don't like this many if statements but can't seem to think of a better way
             */
            if (data.ContainsKey(fieldDefinition.Id))
            {
                var value = data[fieldDefinition.Id];
                if (fieldDefinition.Type == FieldType.Text && string.IsNullOrWhiteSpace(value))
                {
                    return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} cannot be empty.");


                }
                if (fieldDefinition.Type == FieldType.Number)
                {
                    double number;
                    if (!double.TryParse(value, out number))
                    {
                        return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be a valid number.");

                    }
                }

                if (fieldDefinition.Type == FieldType.Date)
                {
                    DateTime date;
                    if (!DateTime.TryParse(value, out date))
                    {
                        return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be a valid date.");

                    }
                }
                if (fieldDefinition.Type == FieldType.Boolean)
                {
                    bool boolean;
                    if (!bool.TryParse(value, out boolean))
                    {
                        return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be a valid boolean (true/false).");
                    }
                }
                if (fieldDefinition.Type == FieldType.Choice)
                {
                    if (!(value == "Yes" || value == "No"))
                    {
                        return ResultFactory.Invalid(fieldDefinition, $"{fieldDefinition.Label} must be one of the following choices: {string.Join(", ", fieldDefinition.Constraints)}.");

                    }
                }
            }


            return ResultFactory.Valid(fieldDefinition, "Valid"); // If all checks pass, return valid
        }
    }
}
