//HintName: S2.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S2 : Serde.ISerialize<S2>
{
    void ISerialize<S2>.Serialize(S2 value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S2", 1);
        type.SerializeField<ColorEnum, global::ColorEnumWrap>("E", value.E);
        type.End();
    }
}