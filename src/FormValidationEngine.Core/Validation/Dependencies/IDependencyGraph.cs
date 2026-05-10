using System.Collections.Generic;

namespace FormValidationEngine.Core.Validation.Dependencies
{
    public interface IDependencyGraph
    {
        void AddNode(string key);
        void AddDependency(string key, string value);
        Dictionary<string, HashSet<string>> GetDependencyGraph();
    }
}