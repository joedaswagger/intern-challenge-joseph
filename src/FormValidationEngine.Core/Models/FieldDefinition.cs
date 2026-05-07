using System.Collections.Generic;
using Newtonsoft.Json;

namespace FormValidationEngine.Core.Models
{
    /// <summary>
    /// Defines a single field on a form, including its type, constraints, and dependencies.
    /// </summary>
    public class FieldDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public FieldType Type { get; set; }

        /// <summary>
        /// Whether this field is required (unconditionally).
        /// If the field has a conditional requirement via <see cref="DependsOn"/>, 
        /// this may be false while still being conditionally required.
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <summary>
        /// IDs of other fields that this field depends on.
        /// A field dependency means: this field's validation cannot run 
        /// until all dependent fields have been validated first.
        /// </summary>
        [JsonProperty("dependsOn")]
        public List<string> DependsOn { get; set; } = new List<string>();

        /// <summary>
        /// Optional condition that determines when this field becomes required.
        /// Example: { "fieldId": "adverse_event_occurred", "operator": "equals", "value": "Yes" }
        /// </summary>
        [JsonProperty("conditionalRequirement")]
        public FieldCondition ConditionalRequirement { get; set; }

        /// <summary>
        /// Validation constraints specific to the field type.
        /// </summary>
        [JsonProperty("constraints")]
        public FieldConstraints Constraints { get; set; }
    }

    public enum FieldType
    {
        Text,
        Number,
        Date,
        Boolean,
        Choice,
        Calculated
    }

    public class FieldCondition
    {
        [JsonProperty("fieldId")]
        public string FieldId { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; } // "equals", "notEquals", "greaterThan", "lessThan", "contains"

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class FieldConstraints
    {
        [JsonProperty("minLength")]
        public int? MinLength { get; set; }

        [JsonProperty("maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty("min")]
        public double? Min { get; set; }

        [JsonProperty("max")]
        public double? Max { get; set; }

        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [JsonProperty("allowedValues")]
        public List<string> AllowedValues { get; set; }

        /// <summary>
        /// For calculated fields: a formula expression referencing other field IDs.
        /// Example: "weight / (height * height)" where weight and height are field IDs.
        /// </summary>
        [JsonProperty("formula")]
        public string Formula { get; set; }
    }
}
