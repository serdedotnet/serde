//HintName: TestCase15.Class1.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class TestCase15
{
    partial class Class1 : Serde.ISerialize<TestCase15.Class1>
    {
        void ISerialize<TestCase15.Class1>.Serialize(TestCase15.Class1 value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Class1>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 0, value.Field0);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 1, value.Field1);
            type.End();
        }
    }
}