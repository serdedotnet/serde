
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Serde;

/// <summary>
/// Helper class to store the results of generation and dedup identical generated files.
/// </summary>
internal sealed class GeneratorExecutionContext
{
    private bool _frozen = false;
    private readonly List<Diagnostic> _diagnostics = new();
    private readonly SortedSet<(string FileName, string Content)> _sources = new();

    public Compilation Compilation { get; }

    public GeneratorExecutionContext(GeneratorAttributeSyntaxContext context)
    {
        Compilation = context.SemanticModel.Compilation;
    }

    public GenerationOutput GetOutput()
    {
        _frozen = true;
        return new GenerationOutput(_diagnostics, _sources);
    }

    public void ReportDiagnostic(Diagnostic diagnostic)
    {
        if (_frozen)
            throw new InvalidOperationException("Cannot add diagnostics after GetDiagnostics() has been called.");
        _diagnostics.Add(diagnostic);
    }

    internal void AddSource(string fileName, string content)
    {
        if (_frozen)
            throw new InvalidOperationException("Cannot add sources after GetSources() has been called.");
        _sources.Add((fileName, content));
    }
}
