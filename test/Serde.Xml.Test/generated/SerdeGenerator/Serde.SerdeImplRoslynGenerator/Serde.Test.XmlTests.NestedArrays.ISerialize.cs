
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class NestedArrays : Serde.ISerializeProvider<Serde.Test.XmlTests.NestedArrays>
        {
            static ISerialize<Serde.Test.XmlTests.NestedArrays> ISerializeProvider<Serde.Test.XmlTests.NestedArrays>.SerializeInstance => NestedArraysSerializeProxy.Instance;

            sealed class NestedArraysSerializeProxy : Serde.ISerialize<Serde.Test.XmlTests.NestedArrays>
            {
                void ISerialize<Serde.Test.XmlTests.NestedArrays>.Serialize(Serde.Test.XmlTests.NestedArrays value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<NestedArrays>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<int[][][], Serde.ArrayProxy.Serialize<int[][], Serde.ArrayProxy.Serialize<int[], Serde.ArrayProxy.Serialize<int, global::Serde.Int32Proxy>>>>(_l_serdeInfo, 0, value.A);
                    type.End();
                }

                public static readonly NestedArraysSerializeProxy Instance = new();
                private NestedArraysSerializeProxy()
                {
                }
            }
        }
    }
}