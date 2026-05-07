using System.Collections.Generic;
using Newtonsoft.Json;

namespace FormValidationEngine.Core.Models
{
    /// <summary>
    /// Represents a complete form definition with all its fields and metadata.
    /// </summary>
    public class FormDefinition
    {
        [JsonProperty("formId")]
        public string FormId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("fields")]
        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
    }
}
