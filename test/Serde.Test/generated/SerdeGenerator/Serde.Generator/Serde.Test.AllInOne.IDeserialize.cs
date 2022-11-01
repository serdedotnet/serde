
#nullable enable
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]{"BoolField", "CharField", "ByteField", "UShortField", "UIntField", "ULongField", "SByteField", "ShortField", "IntField", "LongField", "StringField", "NullStringField", "UIntArr", "NestedArr", "IntImm", "Color"};
            return deserializer.DeserializeType<Serde.Test.AllInOne, SerdeVisitor>("AllInOne", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne";
            Serde.Test.AllInOne Serde.IDeserializeVisitor<Serde.Test.AllInOne>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<bool> boolfield = default;
                Serde.Option<char> charfield = default;
                Serde.Option<byte> bytefield = default;
                Serde.Option<ushort> ushortfield = default;
                Serde.Option<uint> uintfield = default;
                Serde.Option<ulong> ulongfield = default;
                Serde.Option<sbyte> sbytefield = default;
                Serde.Option<short> shortfield = default;
                Serde.Option<int> intfield = default;
                Serde.Option<long> longfield = default;
                Serde.Option<string> stringfield = default;
                Serde.Option<string?> nullstringfield = default;
                Serde.Option<uint[]> uintarr = default;
                Serde.Option<int[][]> nestedarr = default;
                Serde.Option<System.Collections.Immutable.ImmutableArray<int>> intimm = default;
                Serde.Option<Serde.Test.AllInOne.ColorEnum> color = default;
                while (d.TryGetNextKey<D, string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case "boolField":
                            boolfield = d.GetNextValue<D, bool, BoolWrap>();
                            break;
                        case "charField":
                            charfield = d.GetNextValue<D, char, CharWrap>();
                            break;
                        case "byteField":
                            bytefield = d.GetNextValue<D, byte, ByteWrap>();
                            break;
                        case "uShortField":
                            ushortfield = d.GetNextValue<D, ushort, UInt16Wrap>();
                            break;
                        case "uIntField":
                            uintfield = d.GetNextValue<D, uint, UInt32Wrap>();
                            break;
                        case "uLongField":
                            ulongfield = d.GetNextValue<D, ulong, UInt64Wrap>();
                            break;
                        case "sByteField":
                            sbytefield = d.GetNextValue<D, sbyte, SByteWrap>();
                            break;
                        case "shortField":
                            shortfield = d.GetNextValue<D, short, Int16Wrap>();
                            break;
                        case "intField":
                            intfield = d.GetNextValue<D, int, Int32Wrap>();
                            break;
                        case "longField":
                            longfield = d.GetNextValue<D, long, Int64Wrap>();
                            break;
                        case "stringField":
                            stringfield = d.GetNextValue<D, string, StringWrap>();
                            break;
                        case "nullStringField":
                            nullstringfield = d.GetNextValue<D, string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                            break;
                        case "uIntArr":
                            uintarr = d.GetNextValue<D, uint[], ArrayWrap.DeserializeImpl<uint, UInt32Wrap>>();
                            break;
                        case "nestedArr":
                            nestedarr = d.GetNextValue<D, int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case "intImm":
                            intimm = d.GetNextValue<D, System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case "color":
                            color = d.GetNextValue<D, Serde.Test.AllInOne.ColorEnum, AllInOneColorEnumWrap>();
                            break;
                        default:
                            break;
                    }
                }

                var newType = new Serde.Test.AllInOne()
                {BoolField = boolfield.GetValueOrThrow("BoolField"), CharField = charfield.GetValueOrThrow("CharField"), ByteField = bytefield.GetValueOrThrow("ByteField"), UShortField = ushortfield.GetValueOrThrow("UShortField"), UIntField = uintfield.GetValueOrThrow("UIntField"), ULongField = ulongfield.GetValueOrThrow("ULongField"), SByteField = sbytefield.GetValueOrThrow("SByteField"), ShortField = shortfield.GetValueOrThrow("ShortField"), IntField = intfield.GetValueOrThrow("IntField"), LongField = longfield.GetValueOrThrow("LongField"), StringField = stringfield.GetValueOrThrow("StringField"), NullStringField = nullstringfield.GetValueOrDefault(null), UIntArr = uintarr.GetValueOrThrow("UIntArr"), NestedArr = nestedarr.GetValueOrThrow("NestedArr"), IntImm = intimm.GetValueOrThrow("IntImm"), Color = color.GetValueOrThrow("Color"), };
                return newType;
            }
        }
    }
}