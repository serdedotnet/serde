//HintName: Outer.SectionWrap.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class Outer
{
    partial class SectionWrap
    {
        sealed partial class _SerObj : Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Outer.SectionWrap.s_serdeInfo;

            void global::Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>.Serialize(System.Collections.Specialized.BitVector32.Section value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI16(_l_info, 0, value.Mask);
                _l_type.WriteI16(_l_info, 1, value.Offset);
                _l_type.End(_l_info);
            }

        }
    }
}
