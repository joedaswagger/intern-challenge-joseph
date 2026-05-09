using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation
{
    public class DependencyResults : Results
    {
        public List<string> executionOrder { get; set; }
        public DependencyResults(bool success, List<string> errors, List<string> executionOrder) : base(success, errors)
        {
            this.executionOrder = executionOrder;
        }
    }
}
