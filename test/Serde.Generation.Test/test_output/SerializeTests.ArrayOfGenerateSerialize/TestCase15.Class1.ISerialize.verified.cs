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
            var _l_serdeInfo = Class1SerdeInfo.Instance;
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<int, Int32Wrap>(_l_serdeInfo, 0, value.Field0);
            type.SerializeField<byte, ByteWrap>(_l_serdeInfo, 1, value.Field1);
            type.End();
        }
    }
}