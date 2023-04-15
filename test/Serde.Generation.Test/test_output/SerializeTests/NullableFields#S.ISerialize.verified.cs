//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, TSerialize> : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 4);
        type.SerializeFieldIfNotNull("fI", new NullableWrap.SerializeImpl<int, Int32Wrap>(this.FI), this.FI);
        type.SerializeFieldIfNotNull("f3", new NullableWrap.SerializeImpl<TSerialize, IdWrap<TSerialize>>(this.F3), this.F3);
        type.End();
    }
}