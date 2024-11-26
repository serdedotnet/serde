//HintName: Some.Nested.Namespace.ColorULongProxy.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    sealed partial class ColorULongProxy : Serde.ISerialize<Some.Nested.Namespace.ColorULong>, Serde.ISerializeProvider<Some.Nested.Namespace.ColorULong>
    {
        void ISerialize<Some.Nested.Namespace.ColorULong>.Serialize(Some.Nested.Namespace.ColorULong value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorULongProxy>();
            var index = value switch
            {
                Some.Nested.Namespace.ColorULong.Red => 0,
                Some.Nested.Namespace.ColorULong.Green => 1,
                Some.Nested.Namespace.ColorULong.Blue => 2,
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorULong'"),
            };
            serializer.SerializeEnumValue(_l_serdeInfo, index, (ulong)value, global::Serde.UInt64Proxy.Instance);
        }

        static ISerialize<Some.Nested.Namespace.ColorULong> ISerializeProvider<Some.Nested.Namespace.ColorULong>.SerializeInstance => Some.Nested.Namespace.ColorULongProxy.Instance;
    }
}