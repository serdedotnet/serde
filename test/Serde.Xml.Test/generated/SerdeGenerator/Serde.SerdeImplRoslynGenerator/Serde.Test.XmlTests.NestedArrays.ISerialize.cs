
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial class NestedArrays : Serde.ISerializeProvider<Serde.Test.XmlTests.NestedArrays>
    {
        static ISerialize<Serde.Test.XmlTests.NestedArrays> ISerializeProvider<Serde.Test.XmlTests.NestedArrays>.SerializeInstance
            => NestedArraysSerializeProxy.Instance;

        sealed partial class NestedArraysSerializeProxy :Serde.ISerialize<Serde.Test.XmlTests.NestedArrays>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.NestedArrays>.Serialize(Serde.Test.XmlTests.NestedArrays value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<NestedArrays>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteField<int[][][], Serde.ArrayProxy.Serialize<int[][],Serde.ArrayProxy.Serialize<int[],Serde.ArrayProxy.Serialize<int,global::Serde.Int32Proxy>>>>(_l_info, 0, value.A);
                _l_type.End(_l_info);
            }
            public static readonly NestedArraysSerializeProxy Instance = new();
            private NestedArraysSerializeProxy() { }

        }
    }
}
