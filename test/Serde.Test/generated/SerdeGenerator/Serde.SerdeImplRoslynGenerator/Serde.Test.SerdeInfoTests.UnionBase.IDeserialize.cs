
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase>
    {
        static IDeserialize<Serde.Test.SerdeInfoTests.UnionBase> IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase>.DeserializeInstance
            => UnionBaseDeserializeProxy.Instance;

        sealed partial class UnionBaseDeserializeProxy : global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase>
        {
            Serde.Test.SerdeInfoTests.UnionBase IDeserialize<Serde.Test.SerdeInfoTests.UnionBase>.Deserialize(IDeserializer deserializer)
            {
                var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.SerdeInfoTests.UnionBase>();
                var de = deserializer.ReadType(serdeInfo);
                int index;
                if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
                {
                    throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
                }
                Serde.Test.SerdeInfoTests.UnionBase _l_result = index switch {
                    0 => de.ReadValue<Serde.Test.SerdeInfoTests.UnionBase.A, _m_AProxy>(0),
                    1 => de.ReadValue<Serde.Test.SerdeInfoTests.UnionBase.B, _m_BProxy>(1),

                    _ => throw new InvalidOperationException($"Unexpected index: {index}")
                };
                if ((index = de.TryReadIndex(serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    throw Serde.DeserializeException.ExpectedEndOfType(index);
                }
                return _l_result;
            }public static readonly UnionBaseDeserializeProxy Instance = new();
            private UnionBaseDeserializeProxy() { }

        }
    }
}
