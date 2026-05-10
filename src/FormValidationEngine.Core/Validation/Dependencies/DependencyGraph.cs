using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Dependencies
{
    public class DependencyGraph : IDependencyGraph
    {
        public readonly Dictionary<string, HashSet<string>> _dependencyGraph = new Dictionary<string, HashSet<string>>();


        public void AddNode(string key) //Creates empty nodes of IDs to be later filled in if required
        {
            if (!_dependencyGraph.ContainsKey(key))
            {
                _dependencyGraph[key] = new HashSet<string>();
            }
        }

        public Dictionary<string, HashSet<string>> GetDependencyGraph()
        {
            return _dependencyGraph;
        }

        public void AddDependency(string key, string val) //Creates a dependency between two IDs
        {
            AddNode(key);
            AddNode(val);

            _dependencyGraph[key].Add(val);


        }

    }
}
