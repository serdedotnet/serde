//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize<S>
{
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        var _l_typeInfo = SSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<System.Collections.Immutable.ImmutableArray<System.Collections.Specialized.BitVector32.Section>, Serde.ImmutableArrayWrap.SerializeImpl<System.Collections.Specialized.BitVector32.Section, Outer.SectionWrap>>(_l_typeInfo, 0, value.Sections);
        type.End();
    }
}