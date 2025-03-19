//HintName: Some.Nested.Namespace.Base.IDeserialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base : Serde.IDeserializeProvider<Some.Nested.Namespace.Base>
{
    static IDeserialize<Some.Nested.Namespace.Base> IDeserializeProvider<Some.Nested.Namespace.Base>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj : global::Serde.IDeserialize<Some.Nested.Namespace.Base>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base.s_serdeInfo;

        Some.Nested.Namespace.Base IDeserialize<Some.Nested.Namespace.Base>.Deserialize(IDeserializer deserializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var de = deserializer.ReadType(_l_serdeInfo);
            int index;
            if ((index = de.TryReadIndex(_l_serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
            }
            Some.Nested.Namespace.Base _l_result = index switch {
                0 => de.ReadValue<Some.Nested.Namespace.Base.A, _m_AProxy>(_l_serdeInfo, 0),
                1 => de.ReadValue<Some.Nested.Namespace.Base.B, _m_BProxy>(_l_serdeInfo, 1),

                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
            if ((index = de.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                throw Serde.DeserializeException.ExpectedEndOfType(index);
            }
            return _l_result;
        }public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
