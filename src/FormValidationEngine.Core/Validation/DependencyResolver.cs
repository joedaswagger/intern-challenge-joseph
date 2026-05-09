using FormValidationEngine.Core.Exceptions;
using FormValidationEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormValidationEngine.Core.Validation
{
    public class DependencyResolver
    {
        private Dictionary<string, HashSet<string>> _dependencyGraph = new Dictionary<string, HashSet<string>>();


        private void AddNode(string key) //Creates empty nodes of IDs to be later filled in if required
        {
            if (!_dependencyGraph.ContainsKey(key))
            {
                _dependencyGraph[key] = new HashSet<string>();
            }
        }

        private void AddDependency(string key, string val) //Creates a dependency between two IDs
        {
            AddNode(key);
            AddNode(val);

            _dependencyGraph[key].Add(val);
        }


        public DependencyResults Resolve(List<FieldDefinition> fieldDefinitions)
        {

            foreach (var fieldDefinition in fieldDefinitions) //Algorithm for building (O(n + m), where n is the total number of entries and m is the number of dependencies) 
            {
                var currentId = fieldDefinition.Id;
                var dependsOn = fieldDefinition.DependsOn;
                AddNode(currentId);

                foreach (var dependency in dependsOn)
                {
                    AddDependency(currentId, dependency);
                }
            }

            try { //Validate whether the graph has circular dependencies or not, if it does, handle CircularDependencyException
                var check = new DependencyCheck();

                var analysis = check.Analyze(_dependencyGraph);
                DependencyResults result = new DependencyResults(true, new List<string>(), analysis);
                return result;
            }

            catch (Exception e) {
                DependencyResults result = new DependencyResults(false, new List<string>() {e.Message}, new List<string>());
                return result;
            }


        }

    }


    }

    public class DependencyCheck
    {

        private Stack<string> _path = new Stack<string>();

        private List<string> _topologicalOrder = new List<string>();
        public List<string> Analyze(Dictionary<string, HashSet<string>> graph)
        {
            var states = new Dictionary<string, VisitState>();

            foreach (var node in graph.Keys) //Initialize all nodes as unvisited
            {
                states[node] = VisitState.Unvisited;
            }

            foreach (var node in graph.Keys)
            {
                if (states[node] == VisitState.Unvisited) //for all unvisited nodes, check if you can cycle back to it
                {
                    var cycle = Visit(node, graph, states);

                    if (cycle != null)
                    {
                        throw new CircularDependencyException(cycle);
                    }
                }
            }

            return _topologicalOrder;
        }
        private List<string> Visit(string node, Dictionary<string, HashSet<string>> graph, Dictionary<string, VisitState> states) //DFS performed recursively
        {
            states[node] = VisitState.Visiting; //set node being evaluated as visiting
            _path.Push(node);

            foreach (var dependency in graph[node])
            {
                if (!states.ContainsKey(dependency)) //Missing dependency (Do an exception handling for this later maybe?)
                {
                    continue;
                }

                if (states[dependency] == VisitState.Visiting) //If we came back to a node we're still evaluating, it's circular
                {
                    var cycle = _path
                        .Reverse()
                        .SkipWhile(x => x != dependency)
                        .ToList();

                    cycle.Add(dependency);
                    return cycle;
                }

                if (states[dependency] == VisitState.Unvisited) //If we reached an unvisited node, run the method again with the dependency (simulate moving to next node)
                {
                    var cycle = Visit(dependency, graph, states);

                    if (cycle != null)
                    {
                        return cycle;
                    }
                }
            }

            _path.Pop();
            states[node] = VisitState.Visited;
            _topologicalOrder.Add(node);

            return null;
        }
    }

    public enum VisitState
    {
        Unvisited,
        Visiting,
        Visited
    }

