//HintName: TestCase15.Class0.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class TestCase15
{
    partial class Class0 : Serde.ISerialize<TestCase15.Class0>
    {
        void ISerialize<TestCase15.Class0>.Serialize(TestCase15.Class0 value, ISerializer serializer)
        {
            var type = serializer.SerializeType("Class0", 2);
            type.SerializeField<TestCase15.Class1[], Serde.ArrayWrap.SerializeImpl<TestCase15.Class1, IdWrap<TestCase15.Class1>>>("field0", value.Field0);
            type.SerializeField<bool[], Serde.ArrayWrap.SerializeImpl<bool, BoolWrap>>("field1", value.Field1);
            type.End();
        }
    }
}