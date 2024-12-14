//HintName: Serde.Test.AllInOne.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.ISerializeProvider<Serde.Test.AllInOne>
    {
        static ISerialize<Serde.Test.AllInOne> ISerializeProvider<Serde.Test.AllInOne>.SerializeInstance => AllInOneSerializeProxy.Instance;

        sealed class AllInOneSerializeProxy : Serde.ISerialize<Serde.Test.AllInOne>
        {
            void ISerialize<Serde.Test.AllInOne>.Serialize(Serde.Test.AllInOne value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<AllInOne>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<bool, global::Serde.BoolProxy>(_l_serdeInfo, 0, value.BoolField);
                type.SerializeField<char, global::Serde.CharProxy>(_l_serdeInfo, 1, value.CharField);
                type.SerializeField<byte, global::Serde.ByteProxy>(_l_serdeInfo, 2, value.ByteField);
                type.SerializeField<ushort, global::Serde.UInt16Proxy>(_l_serdeInfo, 3, value.UShortField);
                type.SerializeField<uint, global::Serde.UInt32Proxy>(_l_serdeInfo, 4, value.UIntField);
                type.SerializeField<ulong, global::Serde.UInt64Proxy>(_l_serdeInfo, 5, value.ULongField);
                type.SerializeField<sbyte, global::Serde.SByteProxy>(_l_serdeInfo, 6, value.SByteField);
                type.SerializeField<short, global::Serde.Int16Proxy>(_l_serdeInfo, 7, value.ShortField);
                type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 8, value.IntField);
                type.SerializeField<long, global::Serde.Int64Proxy>(_l_serdeInfo, 9, value.LongField);
                type.SerializeField<string, global::Serde.StringProxy>(_l_serdeInfo, 10, value.StringField);
                type.SerializeField<string, global::Serde.StringProxy>(_l_serdeInfo, 11, value.EscapedStringField);
                type.SerializeFieldIfNotNull<string?, Serde.NullableRefProxy.Serialize<string, global::Serde.StringProxy>>(_l_serdeInfo, 12, value.NullStringField);
                type.SerializeField<uint[], Serde.ArrayProxy.Serialize<uint, global::Serde.UInt32Proxy>>(_l_serdeInfo, 13, value.UIntArr);
                type.SerializeField<int[][], Serde.ArrayProxy.Serialize<int[], Serde.ArrayProxy.Serialize<int, global::Serde.Int32Proxy>>>(_l_serdeInfo, 14, value.NestedArr);
                type.SerializeField<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.Serialize<int, global::Serde.Int32Proxy>>(_l_serdeInfo, 15, value.IntImm);
                type.SerializeField<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>(_l_serdeInfo, 16, value.Color);
                type.End();
            }

            public static readonly AllInOneSerializeProxy Instance = new();
            private AllInOneSerializeProxy()
            {
            }
        }
    }
}