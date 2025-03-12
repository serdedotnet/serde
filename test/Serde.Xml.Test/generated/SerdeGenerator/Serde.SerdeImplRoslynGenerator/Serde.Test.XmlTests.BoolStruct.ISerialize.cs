
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial struct BoolStruct : Serde.ISerializeProvider<Serde.Test.XmlTests.BoolStruct>
    {
        static ISerialize<Serde.Test.XmlTests.BoolStruct> ISerializeProvider<Serde.Test.XmlTests.BoolStruct>.SerializeInstance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.XmlTests.BoolStruct>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.BoolStruct>.Serialize(Serde.Test.XmlTests.BoolStruct value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<BoolStruct>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteBool(_l_info, 0, value.BoolField);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
