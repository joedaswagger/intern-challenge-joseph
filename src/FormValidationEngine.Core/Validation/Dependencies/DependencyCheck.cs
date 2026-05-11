using FormValidationEngine.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormValidationEngine.Core.Validation.Dependencies
{
    public class DependencyCheck : IDependencyCheck
    {
        private Stack<string> _path = new Stack<string>();

        private List<string> _topologicalOrder = new List<string>();

        private List<string> _missingDependencies = new List<string>();
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

            if (_missingDependencies.Any()) //If one or more missing dependencies have been found in the cycle, throw an exception with the list of missing dependencies
            {
                throw new Exceptions.MissingFieldException(_missingDependencies);
            }

            return _topologicalOrder; //Upon success, return topological order
        }

        private List<string> Visit(string node, Dictionary<string, HashSet<string>> graph, Dictionary<string, VisitState> states) //DFS performed recursively
        {
            states[node] = VisitState.Visiting; //set node being evaluated as visiting
            _path.Push(node);


            foreach (var dependency in graph[node])
            {
                if (!states.ContainsKey(dependency)) //For misisng dependencies
                {   if(!_missingDependencies.Contains(dependency))
                        _missingDependencies.Add(dependency);
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
}
