
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Xunit;

namespace Serde.Test;

internal static class CompExtensions
{
    public static MetadataReference EmitToImageReference(
        this Compilation comp,
        EmitOptions? options = null) => EmitToPortableExecutableReference(comp, options);

    public static PortableExecutableReference EmitToPortableExecutableReference(this Compilation comp, EmitOptions? options = null)
    {
        var image = comp.EmitToArray(options);
        return AssemblyMetadata.CreateFromImage(image).GetReference();
    }

    internal static ImmutableArray<byte> EmitToArray(this Compilation compilation, EmitOptions? options = null)
    {
        var peStream = new MemoryStream();

        var emitResult = compilation.Emit(peStream: peStream, options: options);

        Assert.True(emitResult.Success, "Failed to emit");

        return peStream.GetBuffer().ToImmutableArray();
    }
}