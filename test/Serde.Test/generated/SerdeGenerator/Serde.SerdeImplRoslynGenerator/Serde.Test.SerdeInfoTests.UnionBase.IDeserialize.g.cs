
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SerdeInfoTests.UnionBase.s_serdeInfo;

            Serde.Test.SerdeInfoTests.UnionBase IDeserialize<Serde.Test.SerdeInfoTests.UnionBase>.Deserialize(IDeserializer deserializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var de = deserializer.ReadType(_l_serdeInfo);
                int index;
                if ((index = de.TryReadIndex(_l_serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
                {
                    throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
                }
                Serde.Test.SerdeInfoTests.UnionBase _l_result = index switch {
                    0 => de.ReadValue<Serde.Test.SerdeInfoTests.UnionBase.A, _m_AProxy>(_l_serdeInfo, 0),
                    1 => de.ReadValue<Serde.Test.SerdeInfoTests.UnionBase.B, _m_BProxy>(_l_serdeInfo, 1),

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
}
