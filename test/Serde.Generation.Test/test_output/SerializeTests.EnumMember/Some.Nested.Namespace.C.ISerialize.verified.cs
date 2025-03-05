//HintName: Some.Nested.Namespace.C.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial class C : Serde.ISerializeProvider<Some.Nested.Namespace.C>
{
    static ISerialize<Some.Nested.Namespace.C> ISerializeProvider<Some.Nested.Namespace.C>.SerializeInstance
        => CSerializeProxy.Instance;

    sealed partial class CSerializeProxy :Serde.ISerialize<Some.Nested.Namespace.C>
    {
        void global::Serde.ISerialize<Some.Nested.Namespace.C>.Serialize(Some.Nested.Namespace.C value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<C>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedField<Some.Nested.Namespace.ColorInt, Some.Nested.Namespace.ColorIntProxy>(_l_info, 0, value.ColorInt);
            _l_type.WriteBoxedField<Some.Nested.Namespace.ColorByte, Some.Nested.Namespace.ColorByteProxy>(_l_info, 1, value.ColorByte);
            _l_type.WriteBoxedField<Some.Nested.Namespace.ColorLong, Some.Nested.Namespace.ColorLongProxy>(_l_info, 2, value.ColorLong);
            _l_type.WriteBoxedField<Some.Nested.Namespace.ColorULong, Some.Nested.Namespace.ColorULongProxy>(_l_info, 3, value.ColorULong);
            _l_type.End(_l_info);
        }
        public static readonly CSerializeProxy Instance = new();
        private CSerializeProxy() { }

    }
}
