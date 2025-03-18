
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial class NestedArrays : Serde.ISerializeProvider<Serde.Test.XmlTests.NestedArrays>
    {
        static ISerialize<Serde.Test.XmlTests.NestedArrays> ISerializeProvider<Serde.Test.XmlTests.NestedArrays>.SerializeInstance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.XmlTests.NestedArrays>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.NestedArrays>.Serialize(Serde.Test.XmlTests.NestedArrays value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<NestedArrays>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValue<int[][][], Serde.ArrayProxy.Ser<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>>(_l_info, 0, value.A);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
