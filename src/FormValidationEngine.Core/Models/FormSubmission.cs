using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FormValidationEngine.Core.Models
{
    /// <summary>
    /// Represents data submitted for a form.
    /// </summary>
    public class FormSubmission
    {
        [JsonProperty("submissionId")]
        public string SubmissionId { get; set; }

        [JsonProperty("formId")]
        public string FormId { get; set; }

        [JsonProperty("submittedAt")]
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// The submitted field values, keyed by field ID.
        /// Values are always stored as strings; type coercion is part of validation.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}
