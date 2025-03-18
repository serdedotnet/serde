
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU>
    {
        static IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU> IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU>.DeserializeInstance
            => _DeObj.Instance;

        sealed partial class _DeObj : global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU>
        {
            Serde.Test.JsonDeserializeTests.BasicDU IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU>.Deserialize(IDeserializer deserializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.JsonDeserializeTests.BasicDU>();
                var de = deserializer.ReadType(_l_serdeInfo);
                int index;
                if ((index = de.TryReadIndex(_l_serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
                {
                    throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
                }
                Serde.Test.JsonDeserializeTests.BasicDU _l_result = index switch {
                    0 => de.ReadValue<Serde.Test.JsonDeserializeTests.BasicDU.A, _m_AProxy>(_l_serdeInfo, 0),
                    1 => de.ReadValue<Serde.Test.JsonDeserializeTests.BasicDU.B, _m_BProxy>(_l_serdeInfo, 1),

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
}
