using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation
{
    public class Results //Universal Results class (every form of validation will be based off this class)
    {
        public List<string> errors { get; set; }
        public bool success { get; set; }
        
        public Results(bool success, List<string> errors)
        {
            this.errors = errors;
            this.success = success;
        }
    }
}
