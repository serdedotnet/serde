//HintName: TestCase15.Class1.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class TestCase15
{
    partial class Class1 : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("Class1", 2);
            type.SerializeField("field0", new Int32Wrap(this.Field0));
            type.SerializeField("field1", new ByteWrap(this.Field1));
            type.End();
        }
    }
}