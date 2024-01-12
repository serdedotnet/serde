//HintName: C.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial record C : Serde.ISerialize<C>
{
    void ISerialize<C>.Serialize(C value, ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeField<int, Int32Wrap>("x", value.X);
        type.End();
    }
}