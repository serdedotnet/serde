//HintName: Serde.Test.AllInOne.ISerialize.g.cs

#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne
{
    sealed partial class _SerObj : Serde.ISerialize<Serde.Test.AllInOne>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.AllInOne.s_serdeInfo;

        void global::Serde.ISerialize<Serde.Test.AllInOne>.Serialize(Serde.Test.AllInOne value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBool(_l_info, 0, value.BoolField);
            _l_type.WriteChar(_l_info, 1, value.CharField);
            _l_type.WriteU8(_l_info, 2, value.ByteField);
            _l_type.WriteU16(_l_info, 3, value.UShortField);
            _l_type.WriteU32(_l_info, 4, value.UIntField);
            _l_type.WriteU64(_l_info, 5, value.ULongField);
            _l_type.WriteU128(_l_info, 6, value.UInt128Field);
            _l_type.WriteI8(_l_info, 7, value.SByteField);
            _l_type.WriteI16(_l_info, 8, value.ShortField);
            _l_type.WriteI32(_l_info, 9, value.IntField);
            _l_type.WriteI64(_l_info, 10, value.LongField);
            _l_type.WriteI128(_l_info, 11, value.Int128Field);
            _l_type.WriteString(_l_info, 12, value.StringField);
            _l_type.WriteBoxedValue<global::System.DateTimeOffset, global::Serde.DateTimeOffsetProxy>(_l_info, 13, value.DateTimeOffsetField);
            _l_type.WriteDateTime(_l_info, 14, value.DateTimeField);
            _l_type.WriteBoxedValue<global::System.DateOnly, global::Serde.DateOnlyProxy>(_l_info, 15, value.DateOnlyField);
            _l_type.WriteBoxedValue<global::System.TimeOnly, global::Serde.TimeOnlyProxy>(_l_info, 16, value.TimeOnlyField);
            _l_type.WriteBoxedValue<global::System.Guid, global::Serde.GuidProxy>(_l_info, 17, value.GuidField);
            _l_type.WriteString(_l_info, 18, value.EscapedStringField);
            _l_type.WriteStringIfNotNull(_l_info, 19, value.NullStringField);
            _l_type.WriteValue<uint[], Serde.ArrayProxy.Ser<uint, global::Serde.U32Proxy>>(_l_info, 20, value.UIntArr);
            _l_type.WriteValue<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>(_l_info, 21, value.NestedArr);
            _l_type.WriteValue<global::System.Byte[], global::Serde.ByteArrayProxy>(_l_info, 22, value.ByteArr);
            _l_type.WriteBoxedValue<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 23, value.IntImm);
            _l_type.WriteBoxedValue<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>(_l_info, 24, value.Color);
            _l_type.End(_l_info);
        }

    }
}
