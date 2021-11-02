
using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateSerializeAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateDeserializeAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateWrapper : Attribute
    {
        public GenerateWrapper(string memberName) { }
    }

}