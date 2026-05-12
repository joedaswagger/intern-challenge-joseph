using FormValidationEngine.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class CrossFieldValidation : IFieldValidation
    {
        private DateTime _submissionDate;
        public CrossFieldValidation(DateTime submissionDate) { 
            _submissionDate = submissionDate;
        }
        public bool CanValidate(FieldDefinition fieldDefinition)
        {
            if(fieldDefinition.Type == FieldType.Date)
            {
                return true; 
            } else
            {
                return false;
            }
        }

        public FieldValidationResult Validate(FieldDefinition fieldDefinition, Dictionary<string, string> data)
        {
            if(fieldDefinition.Type == FieldType.Date && data.ContainsKey(fieldDefinition.Id)) // Check whether event date is after visit date
            {
                DateTime date;
                    if (DateTime.TryParse(data["event_date"], out date) && DateTime.TryParse(data["visit_date"], out date))
                    {
                        DateTime eventDate = DateTime.Parse(data["event_date"]);
                        DateTime visitDate = DateTime.Parse(data["visit_date"]);
                        if (visitDate < eventDate)
                        {
                            return ResultFactory.Warning(fieldDefinition, "Event date is after visit date");
                        }

                    }

                    if(DateTime.TryParse(data["event_date"], out date)) //Check if event date is in future
                    {
                        DateTime eventDate = DateTime.Parse(data["event_date"]); 
                        if (eventDate > _submissionDate)
                        {
                            return ResultFactory.Warning(fieldDefinition, "Event date is in the future compared to submission date");
                        }
                }
            }


            
            return ResultFactory.Valid(fieldDefinition, "Valid");
        }
    }
}
