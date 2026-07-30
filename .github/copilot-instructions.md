# GitHub Copilot Instructions

## Proof Files in Gripe.Testing

When adding a new public method to any proof file under `src/Gripe.Testing/`, you **must** include an `<example>` XML documentation block on every public method. Failing to do so will cause the `PublicMethodsShouldHaveDocumentedCodeExamplesAnalyzerTests.ReturnsDiagnosticResults` test to report an unexpected diagnostic and fail.

The `PublicMethodsShouldHaveDocumentedCodeExamplesAnalyzerTests.ReturnsDiagnosticResults` test **MUST NOT** be ignored or worked around. Fix the root cause by adding the required XML comment.

### Required XML comment pattern

```csharp
/// <summary>
/// Brief description of what the proof method demonstrates.
/// </summary>
/// <param name="paramName">Description of the parameter.</param>
/// <example>
/// <code>
/// var instance = new SomeDependency();
/// ProofClass.MethodName(instance);
/// </code>
/// </example>
/// <remarks>
/// This code is just a proof for
/// 1) making sure the code builds
/// 2) making sure the analyzer triggers
///
/// It is in no way meant to be regarded as usable code.
/// </remarks>
public static void MethodName(SomeDependency instance)
```
