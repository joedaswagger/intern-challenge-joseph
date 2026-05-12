using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class RequiredFieldValidation : IFieldValidation
    {
        public bool CanValidate(FieldDefinition fieldDefinition)
        {
            return true;
        }

        public FieldValidationResult Validate(FieldDefinition fieldDefinition, Dictionary<string, string> data) //Two cases where validation fails: 1) The field is required but not present in the submission, 2) The field has a conditional requirement that is not met
        {
            
            if (fieldDefinition.ConditionalRequirement != null && !data.ContainsKey(fieldDefinition.ConditionalRequirement.FieldId))
            {
                return ResultFactory.Invalid(fieldDefinition, $"Conditional requirement '{fieldDefinition.ConditionalRequirement.FieldId}' is not present in the submission");

            }

            if (!data.ContainsKey(fieldDefinition.Id) && fieldDefinition.Required) //If the field is not present in the submission but it's marked as required
                return ResultFactory.Invalid(fieldDefinition, "Field is required but not present in the submission");


            return ResultFactory.Valid(fieldDefinition, "Valid"); //Default
        }


    }
}
