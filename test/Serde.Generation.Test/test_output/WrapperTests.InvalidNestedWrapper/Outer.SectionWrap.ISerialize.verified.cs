//HintName: Outer.SectionWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class Outer
{
    partial class SectionWrap : Serde.ISerializeProvider<System.Collections.Specialized.BitVector32.Section>
    {
        static ISerialize<System.Collections.Specialized.BitVector32.Section> ISerializeProvider<System.Collections.Specialized.BitVector32.Section>.SerializeInstance
            => SectionWrapSerializeProxy.Instance;

        sealed partial class SectionWrapSerializeProxy :Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>
        {
            void global::Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>.Serialize(System.Collections.Specialized.BitVector32.Section value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<SectionWrap>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<short,global::Serde.Int16Proxy>(_l_serdeInfo,0,value.Mask);
                type.SerializeField<short,global::Serde.Int16Proxy>(_l_serdeInfo,1,value.Offset);
                type.End();
            }
            public static readonly SectionWrapSerializeProxy Instance = new();
            private SectionWrapSerializeProxy() { }

        }
    }
}
