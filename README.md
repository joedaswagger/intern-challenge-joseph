# Form Validation Engine — Intern Coding Challenge

## Overview

You are building a **Form Validation Engine** for a clinical data capture system.

In clinical trials, researchers collect data through electronic forms (Case Report Forms). These forms have fields with complex interdependencies — for example, "Date of Adverse Event" is only required if the participant reported that an adverse event occurred. Validation must respect these dependencies: you cannot validate a dependent field until its prerequisite fields have been validated first.

Your task is to implement the core validation pipeline that:

1. Resolves the correct execution order of field validations based on their dependency graph
2. Detects circular dependencies and reports them clearly
3. Executes field validations in the resolved order
4. Produces a structured validation report

## Time Expectation

This challenge is designed to be completed in **one working day** (6–8 hours). Focus on code quality over feature completeness — a well-structured partial solution is better than a rushed complete one.

## Repository Structure

```
intern-challenge/
├── README.md                  ← You are here
├── docs/
│   ├── sample-form.json              # Example form definition
│   ├── sample-submission.json        # Valid submission data
│   ├── sample-submission-invalid.json # Invalid submission data
│   ├── sample-form-circular.json     # Form with circular dependencies (should fail gracefully)
│   ├── expected-output-valid.json    # Expected output for valid submission
│   └── expected-output-invalid.json  # Expected output for invalid submission
└── src/
    ├── FormValidationEngine.sln
    ├── FormValidationEngine.Core/    # .NET Standard 2.0 class library (YOUR MAIN WORK)
    │   ├── Models/                   # Data models (provided)
    │   └── Exceptions/               # Custom exceptions (provided)
    └── FormValidationEngine.App/     # .NET 8 console app (entry point provided)
```

## What's Provided

- **Models**: `FormDefinition`, `FieldDefinition`, `FormSubmission`, `ValidationReport` and related types
- **Exception**: `CircularDependencyException`
- **Console App**: A minimal .NET 8 console application that reads a form definition and submission from JSON files
- **Sample Data**: Example form definitions and submissions with expected outputs

## What You Need to Implement

### 1. Dependency Resolution

Implement a dependency resolver that:
- Accepts a collection of field definitions
- Analyzes the `dependsOn` relationships to build a dependency graph
- Returns a valid execution order (topological sort)
- Throws `CircularDependencyException` with the cycle path when circular dependencies are detected

**Hint**: Each field's `dependsOn` list declares which other fields must be validated *before* this field. Think of it as a directed acyclic graph (DAG) where an edge from A → B means "B depends on A" (A must come first).

### 2. Field Validation

Implement validators following the Single Responsibility Principle. Each validator handles one concern:

- **Required Field Validator**: Checks if required fields (and conditionally required fields) have values
- **Type Validator**: Verifies values match their declared type (number, date, etc.)
- **Constraint Validator**: Checks min/max, minLength/maxLength, pattern, allowedValues
- **Calculated Field Validator** (bonus): Evaluates formula expressions for calculated fields

You may add more validators as you see fit.

### 3. Validation Pipeline

Implement the orchestration layer that:
- Uses the dependency resolver to determine execution order
- Iterates through fields in resolved order
- Applies all applicable validators to each field
- Handles the case where dependency resolution fails (circular dependency)
- Collects results into a `ValidationReport`

### 4. Application Wiring

Wire everything together in `Program.cs` so the console application functions end-to-end.

## Running the Application

```bash
cd src/FormValidationEngine.App
dotnet run -- ../../docs/sample-form.json ../../docs/sample-submission.json
```

The application should output a JSON `ValidationReport` to stdout.

## Requirements & Constraints

| Requirement | Details |
|-------------|---------|
| **Core Library Target** | .NET Standard 2.0 (ensuring compatibility with both .NET Framework 4.7.2+ and modern .NET) |
| **Console App Target** | .NET 8 |
| **JSON Library** | Newtonsoft.Json (already referenced) |
| **Architecture** | Follow SOLID principles. Code should be extensible without modification (Open/Closed). |
| **Error Handling** | Graceful degradation. Never crash on bad input — report errors in the validation report. |

## Evaluation Criteria

We will evaluate your submission on:

1. **Correctness**: Does the engine produce correct validation results? Does dependency resolution work?
2. **Code Quality**: Is the code clean, readable, and well-organized? Appropriate naming? Single Responsibility?
3. **Architecture**: Is the code well-structured? Is it easy to extend, test, and maintain?
4. **Theoretical Understanding**: Is the dependency resolution algorithm correct and efficient?
5. **Edge Cases**: How does the code handle malformed input, missing fields, circular dependencies?

## Bonus Points

These are not required, but will strengthen your submission:

- Support for the `Calculated` field type (evaluating arithmetic formulas)
- Cross-field validation (e.g., event_date should not be before visit_date)
- Logging
- Unit tests
- A `README` or design notes explaining your architectural decisions

## Example

### Input: Form Definition (simplified)

```json
{
  "fields": [
    { "id": "weight_kg", "type": "Number", "required": true, "dependsOn": [] },
    { "id": "height_cm", "type": "Number", "required": true, "dependsOn": [] },
    { "id": "bmi", "type": "Calculated", "dependsOn": ["weight_kg", "height_cm"],
      "constraints": { "formula": "weight_kg / ((height_cm / 100) * (height_cm / 100))" }
    }
  ]
}
```

### Expected Behavior

1. **Resolve order**: `weight_kg` → `height_cm` → `bmi` (or `height_cm` → `weight_kg` → `bmi`; either is valid since weight and height have no mutual dependency)
2. **Validate** `weight_kg` and `height_cm` first (type check, range check)
3. **Validate** `bmi` last (calculate from validated values)

### Circular Dependency Case

If field A depends on B, B depends on C, and C depends on A — the engine must detect this and throw `CircularDependencyException` with the cycle path (e.g., `["field_a", "field_b", "field_c", "field_a"]`).

## Submission

Please submit:
- Your completed source code (the entire `intern-challenge/` folder)
- Ensure `dotnet build` succeeds with no errors
- Ensure `dotnet run -- <form> <submission>` produces valid JSON output

Good luck! 🧪
