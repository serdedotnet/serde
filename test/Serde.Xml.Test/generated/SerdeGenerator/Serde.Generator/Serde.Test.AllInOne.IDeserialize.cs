
#nullable enable
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static System.Threading.Tasks.ValueTask<Serde.Test.AllInOne> Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize<D>(D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]{"BoolField", "CharField", "ByteField", "UShortField", "UIntField", "ULongField", "SByteField", "ShortField", "IntField", "LongField", "StringField", "NullStringField", "UIntArr", "NestedArr", "IntImm", "Color"};
            return deserializer.DeserializeType<Serde.Test.AllInOne, SerdeVisitor>("AllInOne", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne";
            async System.Threading.Tasks.ValueTask<Serde.Test.AllInOne> Serde.IDeserializeVisitor<Serde.Test.AllInOne>.VisitDictionary<D>(D d)
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
                while (await d.TryGetNextKey<string, StringWrap>()is var nextOpt && nextOpt.HasValue)
                {
                    switch (nextOpt.GetValueOrDefault())
                    {
                        case "boolField":
                            boolfield = await d.GetNextValue<bool, BoolWrap>();
                            break;
                        case "charField":
                            charfield = await d.GetNextValue<char, CharWrap>();
                            break;
                        case "byteField":
                            bytefield = await d.GetNextValue<byte, ByteWrap>();
                            break;
                        case "uShortField":
                            ushortfield = await d.GetNextValue<ushort, UInt16Wrap>();
                            break;
                        case "uIntField":
                            uintfield = await d.GetNextValue<uint, UInt32Wrap>();
                            break;
                        case "uLongField":
                            ulongfield = await d.GetNextValue<ulong, UInt64Wrap>();
                            break;
                        case "sByteField":
                            sbytefield = await d.GetNextValue<sbyte, SByteWrap>();
                            break;
                        case "shortField":
                            shortfield = await d.GetNextValue<short, Int16Wrap>();
                            break;
                        case "intField":
                            intfield = await d.GetNextValue<int, Int32Wrap>();
                            break;
                        case "longField":
                            longfield = await d.GetNextValue<long, Int64Wrap>();
                            break;
                        case "stringField":
                            stringfield = await d.GetNextValue<string, StringWrap>();
                            break;
                        case "nullStringField":
                            nullstringfield = await d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                            break;
                        case "uIntArr":
                            uintarr = await d.GetNextValue<uint[], ArrayWrap.DeserializeImpl<uint, UInt32Wrap>>();
                            break;
                        case "nestedArr":
                            nestedarr = await d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case "intImm":
                            intimm = await d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case "color":
                            color = await d.GetNextValue<Serde.Test.AllInOne.ColorEnum, AllInOneColorEnumWrap>();
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