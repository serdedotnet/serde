//HintName: TestCase15.Class0.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class TestCase15
{
    partial class Class0 : Serde.ISerializeProvider<TestCase15.Class0>
    {
        static ISerialize<TestCase15.Class0> ISerializeProvider<TestCase15.Class0>.SerializeInstance
            => Class0SerializeProxy.Instance;

        sealed partial class Class0SerializeProxy :Serde.ISerialize<TestCase15.Class0>
        {
            void global::Serde.ISerialize<TestCase15.Class0>.Serialize(TestCase15.Class0 value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Class0>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteField<TestCase15.Class1[], Serde.ArrayProxy.Serialize<TestCase15.Class1,TestCase15.Class1>>(_l_info, 0, value.Field0);
                _l_type.WriteField<bool[], Serde.ArrayProxy.Serialize<bool,global::Serde.BoolProxy>>(_l_info, 1, value.Field1);
                _l_type.End(_l_info);
            }
            public static readonly Class0SerializeProxy Instance = new();
            private Class0SerializeProxy() { }

        }
    }
}
