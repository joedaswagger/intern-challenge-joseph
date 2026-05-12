namespace FormValidationEngine.Tests;

using FormValidationEngine.Core.Models;
using FormValidationEngine.Core.Validation.Fields;
using FormValidationEngine.Core.Validation.Fields.Types;
using System.Reflection.Metadata;
using Xunit;
public class TypeValidatorTest
{   
    
    
    private readonly Dictionary<string, string> data = new Dictionary<string, string> // created data manually here since we're not manipulating anything, just checking
    {
        { "text", "Sample Text" },
        { "date", "2021-01-01" },
        { "choice", "Yes" },
        { "boolean", "true" },
        { "number", "3" }
    };

    private readonly Dictionary<string, string> invalidData = new Dictionary<string, string>
    {
        { "text", "" },
        { "date", "invalid-date" },
        { "choice", "" },
        { "boolean", "not-boolean" },
        { "number", "not-a-number" }
    };

    [Theory]
    [MemberData(nameof(TestDefinitions))]
    public void Type_Validator_Test(Core.Models.FieldDefinition def) // The way that the text validation is written allows for all methods to be reached in one test
    {
        TypeValidation validator = new TypeValidation(new List<ITypeStrategy> { new TextTypeStrategy(), new NumberTypeStrategy(), new DateTypeStrategy(), new ChoiceTypeStrategy(), new BooleanTypeStrategy() });
        var result = validator.Validate(def, data);

        Assert.Equal("Valid", result.Message);
        Assert.True(result.IsValid);

    }

    [Theory]
    [MemberData(nameof(TestDefinitions))]
    public void Type_Validator_Invalid_Test(Core.Models.FieldDefinition def)
    {
        TypeValidation validator = new TypeValidation(new List<ITypeStrategy> { new TextTypeStrategy(), new NumberTypeStrategy(), new DateTypeStrategy(), new ChoiceTypeStrategy(), new BooleanTypeStrategy() });
        var result = validator.Validate(def, invalidData);

        Assert.False(result.IsValid);
    }

    public static IEnumerable<object[]> TestDefinitions()
    {
        yield return new object[] { new Core.Models.FieldDefinition { Id = "text", Type = FieldType.Text } };
        yield return new object[] { new Core.Models.FieldDefinition {Id = "date", Type = FieldType.Date } };
        yield return new object[] { new Core.Models.FieldDefinition { Id = "choice", Type = FieldType.Choice } };
        yield return new object[] { new Core.Models.FieldDefinition { Id = "boolean", Type = FieldType.Boolean } };
        yield return new object[] { new Core.Models.FieldDefinition { Id = "number", Type = FieldType.Number } };
    }
}
