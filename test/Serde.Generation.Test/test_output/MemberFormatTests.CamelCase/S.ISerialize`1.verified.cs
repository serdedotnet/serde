//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize<S>
{
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 2);
        type.SerializeField<int, Int32Wrap>("one", value.One);
        type.SerializeField<int, Int32Wrap>("twoWord", value.TwoWord);
        type.End();
    }
}