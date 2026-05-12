using FormValidationEngine.Core.Validation.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidationEngine.Tests
{
    public class DependencyGraphTest
    {
        private readonly DependencyGraph _graphMethod;

        public DependencyGraphTest()
        {
            _graphMethod = new DependencyGraph();
        }
        [Fact]
        public void TestAddNode()
        {
            string key = "field1";
            _graphMethod.AddNode(key);

            var result = _graphMethod.GetDependencyGraph();
            Assert.True(result.ContainsKey(key));

        }

        [Fact]
        public void TestAddDependency()
        {
            string key1 = "field1";
            string key2 = "field2";
            _graphMethod.AddDependency(key1, key2);
            var result = _graphMethod.GetDependencyGraph();
            Assert.True(result.ContainsKey(key1));
            Assert.True(result.ContainsKey(key2));
            Assert.Contains(key2, result[key1]);
        }
    }
}
