//HintName: TestCase15.Class1.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class TestCase15
{
    partial class Class1 : Serde.ISerialize<TestCase15.Class1>
    {
        void ISerialize<TestCase15.Class1>.Serialize(TestCase15.Class1 value, ISerializer serializer)
        {
            var type = serializer.SerializeType("Class1", 2);
            type.SerializeField<int, Int32Wrap>("field0", value.Field0);
            type.SerializeField<byte, ByteWrap>("field1", value.Field1);
            type.End();
        }
    }
}