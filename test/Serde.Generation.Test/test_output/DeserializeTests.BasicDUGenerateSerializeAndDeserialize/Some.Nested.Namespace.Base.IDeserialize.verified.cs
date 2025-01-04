//HintName: Some.Nested.Namespace.Base.IDeserialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base : Serde.IDeserializeProvider<Some.Nested.Namespace.Base>
{
    static IDeserialize<Some.Nested.Namespace.Base> IDeserializeProvider<Some.Nested.Namespace.Base>.DeserializeInstance
        => BaseDeserializeProxy.Instance;

    sealed partial class BaseDeserializeProxy : global::Serde.IDeserialize<Some.Nested.Namespace.Base>
    {
        Some.Nested.Namespace.Base IDeserialize<Some.Nested.Namespace.Base>.Deserialize(IDeserializer deserializer)
        {
            var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Some.Nested.Namespace.Base>();
            var de = deserializer.ReadType(serdeInfo);
            int index;
            if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
            }
            Some.Nested.Namespace.Base _l_result = index switch {
                0 => de.ReadValue<Some.Nested.Namespace.Base.A, _m_AProxy>(0),
                1 => de.ReadValue<Some.Nested.Namespace.Base.B, _m_BProxy>(1),

                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
            if ((index = de.TryReadIndex(serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                throw Serde.DeserializeException.ExpectedEndOfType(index);
            }
            return _l_result;
        }public static readonly BaseDeserializeProxy Instance = new();
        private BaseDeserializeProxy() { }

    }
}
