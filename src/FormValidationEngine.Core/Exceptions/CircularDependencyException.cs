using System;
using System.Collections.Generic;

namespace FormValidationEngine.Core.Exceptions
{
    /// <summary>
    /// Thrown when a circular dependency is detected in the field dependency graph.
    /// </summary>
    public class CircularDependencyException : Exception
    {
        /// <summary>
        /// The field IDs that form the circular dependency chain.
        /// </summary>
        public IReadOnlyList<string> Cycle { get; }

        public CircularDependencyException(IReadOnlyList<string> cycle)
            : base($"Circular dependency detected: {string.Join(" -> ", cycle)}")
        {
            Cycle = cycle;
        }

        public CircularDependencyException(string message) : base(message)
        {
            Cycle = new List<string>();
        }
    }
}
