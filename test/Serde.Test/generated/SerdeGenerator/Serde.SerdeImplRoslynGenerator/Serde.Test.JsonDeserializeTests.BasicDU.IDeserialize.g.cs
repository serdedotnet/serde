
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.BasicDU.s_serdeInfo;

            async global::System.Threading.Tasks.Task<Serde.Test.JsonDeserializeTests.BasicDU> IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU>.Deserialize(IDeserializer deserializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var de = deserializer.ReadType(_l_serdeInfo);
                var (index, errorName) = await de.TryReadIndexWithName(_l_serdeInfo);
                if (index == ITypeDeserializer.IndexNotFound)
                {
                    throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
                }
                Serde.Test.JsonDeserializeTests.BasicDU _l_result = index switch {
                    0 => await de.ReadValue<Serde.Test.JsonDeserializeTests.BasicDU.A, _m_AProxy>(_l_serdeInfo, 0),
                    1 => await de.ReadValue<Serde.Test.JsonDeserializeTests.BasicDU.B, _m_BProxy>(_l_serdeInfo, 1),

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
}
