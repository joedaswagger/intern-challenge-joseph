using FormValidationEngine.Core.Exceptions;
using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormValidationEngine.Core.Validation.Dependencies
{

    public class DependencyResolutionOrchestrator
    {
        private readonly IDependencyGraph _graph;
        private readonly IDependencyCheck _check;
        private List<string> _presentFields;
        private readonly ILogging _logger;
        public DependencyResolutionOrchestrator(ILogging logger) : this(new DependencyGraph(), new DependencyCheck(), logger) //Default constructor, since we don't have any graphs or checks to give from the orchestrator
        {
            _logger = logger;
        }

        public DependencyResolutionOrchestrator(IDependencyGraph graph, IDependencyCheck checker, ILogging logger)
        {
            _graph = graph;
            _check = checker;
            _logger = logger;
        }

        public DependencyResults Resolve(List<FieldDefinition> fieldDefinitions)
        {


            _logger.Info("Building Dependency Graph...");
            try
            { //Validate whether the graph has circular dependencies/missing dependencies or not, if it does, handle it
                _presentFields = fieldDefinitions.ToList().Select(fd => fd.Id).ToList(); //List of all present fields, used to check for missing dependencies
                foreach (var fieldDefinition in fieldDefinitions) //Algorithm for building (O(n + m), where n is the total number of entries and m is the number of dependencies) 
                {
                    var currentId = fieldDefinition.Id;
                    var dependsOn = fieldDefinition.DependsOn;
                    _graph.AddNode(currentId);

                    foreach (var dependency in dependsOn)
                    {
                        if (!_presentFields.Contains(dependency)) //If the dependency exists, add it to the graph, if not, log it as a missing dependency
                        {
                            throw new Exceptions.MissingFieldException(new List<string>() { dependency });

                        }
                        _graph.AddDependency(currentId, dependency);
                    }
                }
                _logger.Info("Graph built. Analyzing...");
                var analysis = _check.Analyze(_graph.GetDependencyGraph());
                return new DependencyResults(true, new List<string>(), analysis);
            }

            catch (Exception e)
            {
                _logger.Error($"Error resolving dependencies: {e.Message}");
                return new DependencyResults(false, new List<string>() { e.Message }, new List<string>());
            }


        }

    }
}


