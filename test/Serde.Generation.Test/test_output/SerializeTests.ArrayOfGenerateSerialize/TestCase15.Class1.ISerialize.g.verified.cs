//HintName: TestCase15.Class1.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class TestCase15
{
    partial class Class1
    {
        sealed partial class _SerObj : Serde.ISerialize<TestCase15.Class1>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => TestCase15.Class1.s_serdeInfo;

            void global::Serde.ISerialize<TestCase15.Class1>.Serialize(TestCase15.Class1 value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.Field0);
                _l_type.WriteU8(_l_info, 1, value.Field1);
                _l_type.End(_l_info);
            }

        }
    }
}
