
using System.Collections.Generic;

namespace FormValidationEngine.Core.Validation
{
    public interface IDependencyCheck
    {
        List<string> Analyze(Dictionary<string, HashSet<string>> graph);
    }
}