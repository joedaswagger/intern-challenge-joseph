using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Exceptions
{
    /// <summary>
    /// Thrown when one or more missing fields are found in the dependency graph or in the submission data.
    /// </summary>
    public class MissingFieldException : Exception
    {
        /// <summary>
        /// The list of missing field IDs.
        /// </summary>
        public IReadOnlyList<string> Field { get; }

        public MissingFieldException(IReadOnlyList<string> fields)
            : base($"Missing field(s) or dependency(s): {string.Join(", ", fields)}. Please check your form and/or submission.")
        {
            Field = fields;
        }

        public MissingFieldException(string message) : base(message)
        {
            Field = new List<string>();
        }
    }
}
