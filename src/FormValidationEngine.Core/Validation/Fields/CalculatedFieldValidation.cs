using FormValidationEngine.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;

namespace FormValidationEngine.Core.Validation.Fields
{
    public class CalculatedFieldValidation : IFieldValidation
    {
        public bool CanValidate(FieldDefinition fieldDefinition)
        {
            if(fieldDefinition.Type == FieldType.Calculated) // This validation is only for calculated fields
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<string> _missingDependencies = new List<string>();
        public FieldValidationResult Validate(FieldDefinition fieldDefinition, Dictionary<string, string> data)
        {

            var formula = fieldDefinition.Constraints?.Formula;
            var dependencies = ExtractDependencies(formula);

            foreach (var dependency in dependencies)
            {
                if (!data.ContainsKey(dependency)) // If dependency(ies) does not exist, throw exception
                {
                    _missingDependencies.Add(dependency);
                }
            }

            if(_missingDependencies.Any())
            {
                throw new Exceptions.MissingFieldException(_missingDependencies);
            }

            formula = AddBrackets(formula, dependencies); // Proper formatting for NCalc (all variables have to have brackets)

            try
            {
                var expression = new Expression(formula);
                foreach (var dependency in dependencies)
                {
                    if (double.TryParse(data[dependency], out double value))
                    {
                        expression.Parameters[dependency] = value;
                    }
                    else
                    {
                        return ResultFactory.Invalid(fieldDefinition, $"Invalid value for dependency: {dependency}");
                    }
                }
                var result = expression.Evaluate();
                double finalResult = Math.Round(Convert.ToDouble(result), 2);
                


                return ResultFactory.Valid(fieldDefinition, $"Calculated field: {finalResult}");

            }
            catch (Exception ex)
            {
                return ResultFactory.Invalid(fieldDefinition, $"Error evaluating formula: {ex.Message}");
            }

            
        }

        private static IReadOnlyList<string> ExtractDependencies(string formula)
        {
            var matches = Regex.Matches(
                formula,
                @"\b[A-Za-z_][A-Za-z0-9_]*\b");

            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();

        }

        private static string AddBrackets(string formula, IEnumerable<string> knownFieldIds)
        {
            var result = formula;

            foreach (var fieldId in knownFieldIds.OrderByDescending(x => x.Length))
            {
                var escapedFieldId = Regex.Escape(fieldId);

                result = Regex.Replace(result, $@"\b{escapedFieldId}\b", $"[{fieldId}]");
            }

            return result;
        }

        
    }
}
