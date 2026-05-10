using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Models
{
    public class FieldResults : Results
    {
        public List<FieldValidationResult> fields { get; set; }
        public FieldResults(bool success, List<string> errors, List<FieldValidationResult> fields) : base(success, errors)
        {
            this.fields = fields;
        }
    }
}
