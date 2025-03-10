//HintName: TestCase15.Class1.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class TestCase15
{
    partial class Class1 : Serde.ISerializeProvider<TestCase15.Class1>
    {
        static ISerialize<TestCase15.Class1> ISerializeProvider<TestCase15.Class1>.SerializeInstance
            => Class1SerializeProxy.Instance;

        sealed partial class Class1SerializeProxy :Serde.ISerialize<TestCase15.Class1>
        {
            void global::Serde.ISerialize<TestCase15.Class1>.Serialize(TestCase15.Class1 value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Class1>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.Field0);
                _l_type.WriteU8(_l_info, 1, value.Field1);
                _l_type.End(_l_info);
            }
            public static readonly Class1SerializeProxy Instance = new();
            private Class1SerializeProxy() { }

        }
    }
}
