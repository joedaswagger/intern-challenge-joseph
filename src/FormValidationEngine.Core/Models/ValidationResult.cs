using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FormValidationEngine.Core.Models
{
    /// <summary>
    /// The complete result of validating a form submission.
    /// </summary>
    public class ValidationReport
    {
        [JsonProperty("submissionId")]
        public string SubmissionId { get; set; }

        [JsonProperty("formId")]
        public string FormId { get; set; }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("executionOrder")]
        public List<string> ExecutionOrder { get; set; } = new List<string>();

        [JsonProperty("results")]
        public List<FieldValidationResult> Results { get; set; } = new List<FieldValidationResult>();

        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// The validation result for a single field.
    /// </summary>
    public class FieldValidationResult
    {
        [JsonProperty("fieldId")]
        public string FieldId { get; set; }

        [JsonProperty("severity")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ValidationSeverity Severity { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }
    }

    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error
    }
}
