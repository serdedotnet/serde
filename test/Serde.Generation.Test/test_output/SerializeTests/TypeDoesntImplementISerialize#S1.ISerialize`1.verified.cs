//HintName: S1.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S1 : Serde.ISerialize<S1>
{
    void ISerialize<S1>.Serialize(S1 value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S1", 1);
        type.End();
    }
}