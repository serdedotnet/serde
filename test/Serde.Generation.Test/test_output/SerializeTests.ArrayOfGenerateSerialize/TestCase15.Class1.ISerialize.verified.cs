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
            var _l_typeInfo = Class1SerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, value.Field0);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 1, value.Field1);
            type.End();
        }
    }
}