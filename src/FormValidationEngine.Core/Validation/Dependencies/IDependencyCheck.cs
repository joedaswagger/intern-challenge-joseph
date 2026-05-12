
using System.Collections.Generic;

namespace FormValidationEngine.Core.Validation
{
    public interface IDependencyCheck
    {
        List<string> Analyze(IReadOnlyDictionary<string, HashSet<string>> graph);
    }
}