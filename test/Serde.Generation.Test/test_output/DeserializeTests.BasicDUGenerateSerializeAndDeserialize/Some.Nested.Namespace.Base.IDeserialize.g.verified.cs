//HintName: Some.Nested.Namespace.Base.IDeserialize.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    sealed partial class _DeObj : Serde.IDeserialize<Some.Nested.Namespace.Base>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base.s_serdeInfo;

        async global::System.Threading.Tasks.Task<Some.Nested.Namespace.Base> IDeserialize<Some.Nested.Namespace.Base>.Deserialize(IDeserializer deserializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var de = deserializer.ReadType(_l_serdeInfo);
            var (index, errorName) = await de.TryReadIndexWithName(_l_serdeInfo);
            if (index == ITypeDeserializer.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
            }
            Some.Nested.Namespace.Base _l_result = index switch {
                0 => await de.ReadValue<Some.Nested.Namespace.Base.A, _m_AProxy>(_l_serdeInfo, 0),
                1 => await de.ReadValue<Some.Nested.Namespace.Base.B, _m_BProxy>(_l_serdeInfo, 1),

                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
            index = await de.TryReadIndex(_l_serdeInfo);
            if (index != ITypeDeserializer.EndOfType)
            {
                throw Serde.DeserializeException.ExpectedEndOfType(index);
            }
            return _l_result;
        }
    }
}
