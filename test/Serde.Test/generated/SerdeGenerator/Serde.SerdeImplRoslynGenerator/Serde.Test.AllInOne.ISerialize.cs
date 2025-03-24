
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne : Serde.ISerializeProvider<Serde.Test.AllInOne>
{
    static ISerialize<Serde.Test.AllInOne> ISerializeProvider<Serde.Test.AllInOne>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<Serde.Test.AllInOne>
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
            _l_type.WriteI8(_l_info, 6, value.SByteField);
            _l_type.WriteI16(_l_info, 7, value.ShortField);
            _l_type.WriteI32(_l_info, 8, value.IntField);
            _l_type.WriteI64(_l_info, 9, value.LongField);
            _l_type.WriteString(_l_info, 10, value.StringField);
            _l_type.WriteString(_l_info, 11, value.EscapedStringField);
            _l_type.WriteStringIfNotNull(_l_info, 12, value.NullStringField);
            _l_type.WriteValue<uint[], Serde.ArrayProxy.Ser<uint, global::Serde.U32Proxy>>(_l_info, 13, value.UIntArr);
            _l_type.WriteValue<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>(_l_info, 14, value.NestedArr);
            _l_type.WriteBoxedValue<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 15, value.IntImm);
            _l_type.WriteBoxedValue<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>(_l_info, 16, value.Color);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
