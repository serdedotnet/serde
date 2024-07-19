//HintName: Outer.SectionWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class Outer
{
    partial record struct SectionWrap : Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>
    {
        void ISerialize<System.Collections.Specialized.BitVector32.Section>.Serialize(System.Collections.Specialized.BitVector32.Section value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<SectionWrap>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<short, global::Serde.Int16Wrap>(_l_serdeInfo, 0, value.Mask);
            type.SerializeField<short, global::Serde.Int16Wrap>(_l_serdeInfo, 1, value.Offset);
            type.End();
        }
    }
}