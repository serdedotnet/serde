//HintName: Outer.SectionWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class Outer
{
    partial class SectionWrap : Serde.ISerializeProvider<System.Collections.Specialized.BitVector32.Section>
    {
        static ISerialize<System.Collections.Specialized.BitVector32.Section> ISerializeProvider<System.Collections.Specialized.BitVector32.Section>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>
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
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
