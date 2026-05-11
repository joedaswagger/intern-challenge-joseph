using FormValidationEngine.Core.Exceptions;
using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormValidationEngine.Core.Validation.Dependencies
{
    

    public class DependencyResolver
    {
        private readonly IDependencyGraph _graph;
        private readonly IDependencyCheck _check;

        public DependencyResolver() : this(new DependencyGraph(), new DependencyCheck()) //Default constructor, since we don't have any graphs or checks to give from the orchestrator
        {
        }

        public DependencyResolver(IDependencyGraph graph, IDependencyCheck checker)
        {
            _graph = graph;
            _check = checker;
        }
        public DependencyResults Resolve(List<FieldDefinition> fieldDefinitions)
        {
            
            foreach (var fieldDefinition in fieldDefinitions) //Algorithm for building (O(n + m), where n is the total number of entries and m is the number of dependencies) 
            {
                var currentId = fieldDefinition.Id;
                var dependsOn = fieldDefinition.DependsOn;
                _graph.AddNode(currentId);

                foreach (var dependency in dependsOn)
                {
                    _graph.AddDependency(currentId, dependency);
                }
            }

            try
            { //Validate whether the graph has circular dependencies/missing dependencies or not, if it does, handle it

                var analysis = _check.Analyze(_graph.GetDependencyGraph());
                return new DependencyResults(true, new List<string>(), analysis);
            }

            catch (Exception e)
            {
                return new DependencyResults(false, new List<string>() { e.Message }, new List<string>());
            }


        }
    }
}


