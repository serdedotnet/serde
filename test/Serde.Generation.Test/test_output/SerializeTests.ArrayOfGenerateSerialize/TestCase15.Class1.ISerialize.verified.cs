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
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Class1>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,0,value.Field0);
                type.SerializeField<byte,global::Serde.ByteProxy>(_l_serdeInfo,1,value.Field1);
                type.End();
            }
            public static readonly Class1SerializeProxy Instance = new();
            private Class1SerializeProxy() { }

        }
    }
}
