
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU>
    {
        static IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU> IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU>.DeserializeInstance
            => BasicDUDeserializeProxy.Instance;

        sealed partial class BasicDUDeserializeProxy : global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU>
        {
            Serde.Test.JsonDeserializeTests.BasicDU IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU>.Deserialize(IDeserializer deserializer)
            {
                var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.JsonDeserializeTests.BasicDU>();
                var de = deserializer.ReadType(serdeInfo);
                int index;
                if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
                {
                    throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
                }
                Serde.Test.JsonDeserializeTests.BasicDU _l_result = index switch {
                    0 => de.ReadValue<Serde.Test.JsonDeserializeTests.BasicDU.A, _m_AProxy>(0),
                    1 => de.ReadValue<Serde.Test.JsonDeserializeTests.BasicDU.B, _m_BProxy>(1),

                    _ => throw new InvalidOperationException($"Unexpected index: {index}")
                };
                if ((index = de.TryReadIndex(serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    throw Serde.DeserializeException.ExpectedEndOfType(index);
                }
                return _l_result;
            }public static readonly BasicDUDeserializeProxy Instance = new();
            private BasicDUDeserializeProxy() { }

        }
    }
}
