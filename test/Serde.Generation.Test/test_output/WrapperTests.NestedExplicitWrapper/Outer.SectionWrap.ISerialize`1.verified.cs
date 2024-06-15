//HintName: Outer.SectionWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class Outer
{
    partial record struct SectionWrap : Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>
    {
        void ISerialize<System.Collections.Specialized.BitVector32.Section>.Serialize(System.Collections.Specialized.BitVector32.Section value, ISerializer serializer)
        {
            var _l_typeInfo = SectionSerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeField<short, Int16Wrap>(_l_typeInfo, 0, value.Mask);
            type.SerializeField<short, Int16Wrap>(_l_typeInfo, 1, value.Offset);
            type.End();
        }
    }
}