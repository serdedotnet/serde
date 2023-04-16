//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeField("intArr", new ArrayWrap.SerializeImpl<int, Int32Wrap>(this.IntArr));
        type.End();
    }
}