# ADR 001 – ConsiderValueTaskInsteadOfTask analyzer (GR0066)

**Status:** Accepted  
**Date:** 2026-06-02  
**ID:** GR0066

---

## Context

`System.Threading.Tasks.Task<T>` is a class, meaning every completed synchronous return from a
`Task<T>`-returning method must allocate a heap object.  Since .NET Core 2.1, the BCL provides
`System.Threading.Tasks.ValueTask<T>` as a struct alternative that avoids this allocation when the
method can return synchronously.

Performance-sensitive hot paths (e.g. cache-lookup wrappers, guard-clause fast paths, thin adapters)
commonly follow this pattern and are the ideal candidates for `ValueTask<T>`.

This ADR records the decision to add a Roslyn analyzer that detects such methods and emits a
**suggestion-level** diagnostic.

---

## Decision

Add `ConsiderValueTaskInsteadOfTaskAnalyzer` (diagnostic ID **GR0066**) to the
`Gripe.Analyzer.Analyzers.Language` namespace.

The analyzer:

* Fires at `DiagnosticSeverity.Info` (suggestion/refactoring level), never as a warning or error.
* Uses the wording "Consider returning `ValueTask<T>` instead of `Task<T>`…" to communicate that
  this is a **performance hint**, not a correctness rule.
* Is enabled by default so that developers see the suggestion without needing additional configuration.

---

## Motivation

The primary motivation is **allocation avoidance on synchronous completion paths**.

When a `Task<T>`-returning method commonly completes without performing any I/O or async work
(e.g. it returns a cached result or validates arguments) every such call allocates a `Task<T>` object.
Switching to `ValueTask<T>` eliminates this allocation in all cases where the result is available
immediately.

This is a well-known optimisation pattern in the BCL (see `Stream`, `IValueTaskSource`, etc.) and is
explicitly recommended in the [Microsoft docs on `ValueTask`][docs-valuetask].

---

## Heuristics and detection rules

The analyzer uses **conservative** heuristics to keep the false-positive rate low.

### Positive patterns (diagnostic is emitted)

| Pattern | Example | Confidence |
|---------|---------|-----------|
| Non-`async` expression-bodied method returning `Task.FromResult` | `Task<int> Get() => Task.FromResult(_v);` | High |
| Non-`async` block-bodied method where at least one branch returns `Task.FromResult` and the remaining branches call a method (async fallback) | See code example below | High |

```csharp
// Reported: fast-path pattern
private Task<string> GetAsync(string key)
{
    if (_cache.TryGetValue(key, out var value))
        return Task.FromResult(value);   // sync hot path

    return FetchAsync(key);              // async fallback
}
```

### Exclusions (diagnostic is suppressed)

| Exclusion | Reason |
|-----------|--------|
| `public` or `protected` methods | Changing the return type is a source-breaking and binary-breaking API change |
| `abstract` methods | No body to analyse |
| `virtual` methods | Derived classes may override and return stored `Task<T>` instances |
| `override` methods | Must match the base signature |
| Explicit interface implementations | Must match the interface signature |
| Methods that return a stored `Task<T>` (field, property, or local variable) | The `Task<T>` may be cached, shared across calls, or re-awaited by callers |
| `async` methods (any complexity) | The async state machine interacts with `ValueTask<T>` differently; detecting beneficial cases requires deeper analysis and is deferred to a future rule |
| Methods where all return paths are neither `Task.FromResult` nor a method call | Unknown or complex expressions are skipped conservatively |

---

## Trade-offs

### Benefits

* Zero-allocation synchronous paths in private / internal helper code.
* Low risk: the analyzer is restricted to non-public, non-virtual, non-override methods.
* Suggestion-level severity means teams can adopt at their own pace.

### Risks and caveats

`ValueTask<T>` is **not universally better** than `Task<T>`:

1. **Consumption rules are stricter** – a `ValueTask<T>` must be awaited at most once and must not
   be awaited after the producing method has returned.  Violating this can produce incorrect results or
   silent data corruption.

2. **`Task<T>` can be re-awaited and stored** – if callers cache the returned task and await it
   multiple times, switching to `ValueTask<T>` is unsafe.  The analyzer mitigates this by excluding
   methods that return a stored `Task<T>` value.

3. **State-machine overhead** – in `async` methods the `ValueTask<T>` struct can increase the size of
   the state machine, which may outweigh the allocation benefit.  This is why `async` methods are
   excluded from this rule.

4. **Interoperability** – `Task.WhenAll`, `Task.WhenAny`, and similar combinators do not accept
   `ValueTask<T>` directly.  A call to `.AsTask()` is required, which re-introduces an allocation.

5. **API stability** – even for `internal` members, consider whether the method is consumed across
   assembly boundaries (e.g. via `InternalsVisibleTo` in test projects) before switching.

---

## Examples

### Should be reported

```csharp
// Thin wrapper – always synchronous
private Task<int> GetCount() => Task.FromResult(_count);

// Cache hit / async fallback pattern
private Task<string> GetAsync(string key)
{
    if (_cache.TryGetValue(key, out var v))
        return Task.FromResult(v);
    return LoadAsync(key);
}
```

### Should NOT be reported

```csharp
// Public API – signature change is breaking
public Task<int> GetCountAsync() => Task.FromResult(_count);

// Override – must match base
public override Task<int> GetAsync(string key) => Task.FromResult(_v);

// Shared / cached Task<T> – may be re-awaited by callers
private Task<int> GetShared() => _sharedTask;

// Multiple awaits – ValueTask benefit is minimal
private async Task<int> ProcessAsync()
{
    await Step1Async();
    await Step2Async();
    return 1;
}
```

---

## Alternatives considered

| Alternative | Rejected reason |
|-------------|-----------------|
| Warning-level severity | Too noisy; `ValueTask<T>` migration is not always safe or desirable |
| Also detecting `async Task<T>` with early sync return | Detection is less reliable and the state-machine complexity changes the cost/benefit equation; deferred to a separate rule |
| Code fix / refactoring to automatically change the signature | Out of scope for the initial implementation; signature changes require caller updates too |

---

## References

* [Microsoft docs – `ValueTask<T>` usage guidelines][docs-valuetask]
* [Stephen Toub – "Understanding the Whys, Whats, and Whens of ValueTask"][toub-post]
* [Roslyn analyzer authoring guide](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix)

[docs-valuetask]: https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask-1
[toub-post]: https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/
