//HintName: TestCase15.Class0.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class TestCase15
{
    partial class Class0 : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("Class0", 2);
            type.SerializeField("field0", new ArrayWrap.SerializeImpl<TestCase15.Class1, IdWrap<TestCase15.Class1>>(this.Field0));
            type.SerializeField("field1", new ArrayWrap.SerializeImpl<bool, BoolWrap>(this.Field1));
            type.End();
        }
    }
}