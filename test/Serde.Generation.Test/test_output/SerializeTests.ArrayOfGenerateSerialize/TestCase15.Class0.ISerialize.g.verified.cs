//HintName: TestCase15.Class0.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class TestCase15
{
    partial class Class0
    {
        sealed partial class _SerObj : Serde.ISerialize<TestCase15.Class0>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => TestCase15.Class0.s_serdeInfo;

            void global::Serde.ISerialize<TestCase15.Class0>.Serialize(TestCase15.Class0 value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValue<TestCase15.Class1[], Serde.ArrayProxy.Ser<TestCase15.Class1, TestCase15.Class1>>(_l_info, 0, value.Field0);
                _l_type.WriteValue<bool[], Serde.ArrayProxy.Ser<bool, global::Serde.BoolProxy>>(_l_info, 1, value.Field1);
                _l_type.End(_l_info);
            }

        }
    }
}
