//HintName: Some.Nested.Namespace.Base.ISerde.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    sealed partial class _SerdeObj : global::Serde.ISerde<Some.Nested.Namespace.Base>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base.s_serdeInfo;

        void ISerialize<Some.Nested.Namespace.Base>.Serialize(Some.Nested.Namespace.Base value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_serdeInfo);
            switch (value)
            {
                case Some.Nested.Namespace.Base.A c:
                    _l_type.WriteValue<Some.Nested.Namespace.Base.A, _m_AProxy>(_l_serdeInfo, 0, c);
                    break;
                case Some.Nested.Namespace.Base.B c:
                    _l_type.WriteValue<Some.Nested.Namespace.Base.B, _m_BProxy>(_l_serdeInfo, 1, c);
                    break;

            }
            _l_type.End(_l_serdeInfo);
        }Some.Nested.Namespace.Base IDeserialize<Some.Nested.Namespace.Base>.Deserialize(IDeserializer deserializer)
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
        }
    }
}
