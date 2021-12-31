
using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateSerialize : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateDeserialize : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateSerde : Attribute
    { }

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateWrapper : Attribute
    {
        public GenerateWrapper(string memberName) { }
    }
}