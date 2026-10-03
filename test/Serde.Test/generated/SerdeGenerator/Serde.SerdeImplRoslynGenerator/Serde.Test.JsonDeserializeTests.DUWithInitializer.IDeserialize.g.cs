
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record DUWithInitializer
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DUWithInitializer>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.DUWithInitializer.s_serdeInfo;

            Serde.Test.JsonDeserializeTests.DUWithInitializer IDeserialize<Serde.Test.JsonDeserializeTests.DUWithInitializer>.Deserialize(IDeserializer deserializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var de = deserializer.ReadType(_l_serdeInfo);
                var (index, errorName) = de.TryReadIndexWithName(_l_serdeInfo);
                if (index == ITypeDeserializer.IndexNotFound)
                {
                    throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
                }
                Serde.Test.JsonDeserializeTests.DUWithInitializer _l_result = index switch {
                    0 => de.ReadValue<Serde.Test.JsonDeserializeTests.DUWithInitializer.A, _m_AProxy>(_l_serdeInfo, 0),

                    _ => throw new InvalidOperationException($"Unexpected index: {index}")
                };
                index = de.TryReadIndex(_l_serdeInfo);
                if (index != ITypeDeserializer.EndOfType)
                {
                    throw Serde.DeserializeException.ExpectedEndOfType(index);
                }
                de.End(_l_serdeInfo);
                return _l_result;
            }
        }
    }
}
