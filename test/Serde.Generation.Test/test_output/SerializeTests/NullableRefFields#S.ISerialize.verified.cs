//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, T3, T4, T5> : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 5);
        type.SerializeFieldIfNotNull("fS", new NullableRefWrap.SerializeImpl<string, StringWrap>(this.FS), this.FS);
        type.SerializeField("f1", this.F1);
        type.SerializeFieldIfNotNull("f2", this.F2, this.F2);
        type.SerializeFieldIfNotNull("f3", new NullableRefWrap.SerializeImpl<T3, IdWrap<T3>>(this.F3), this.F3);
        type.SerializeFieldIfNotNull("f4", this.F4, this.F4);
        type.End();
    }
}