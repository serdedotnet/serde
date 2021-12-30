
#nullable enable
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]{"BoolField", "CharField", "ByteField", "UShortField", "UIntField", "ULongField", "SByteField", "ShortField", "IntField", "LongField", "StringField", "IntArr", "NestedArr", "IntImm"};
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
                Serde.Option<int[]> intarr = default;
                Serde.Option<int[][]> nestedarr = default;
                Serde.Option<System.Collections.Immutable.ImmutableArray<int>> intimm = default;
                while (d.TryGetNextKey<string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case "BoolField":
                            boolfield = d.GetNextValue<bool, BoolWrap>();
                            break;
                        case "CharField":
                            charfield = d.GetNextValue<char, CharWrap>();
                            break;
                        case "ByteField":
                            bytefield = d.GetNextValue<byte, ByteWrap>();
                            break;
                        case "UShortField":
                            ushortfield = d.GetNextValue<ushort, UInt16Wrap>();
                            break;
                        case "UIntField":
                            uintfield = d.GetNextValue<uint, UInt32Wrap>();
                            break;
                        case "ULongField":
                            ulongfield = d.GetNextValue<ulong, UInt64Wrap>();
                            break;
                        case "SByteField":
                            sbytefield = d.GetNextValue<sbyte, SByteWrap>();
                            break;
                        case "ShortField":
                            shortfield = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case "IntField":
                            intfield = d.GetNextValue<int, Int32Wrap>();
                            break;
                        case "LongField":
                            longfield = d.GetNextValue<long, Int64Wrap>();
                            break;
                        case "StringField":
                            stringfield = d.GetNextValue<string, StringWrap>();
                            break;
                        case "IntArr":
                            intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case "NestedArr":
                            nestedarr = d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case "IntImm":
                            intimm = d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        default:
                            throw new InvalidDeserializeValueException("Unexpected field or property name in type Serde.Test.AllInOne: '" + key + "'");
                    }
                }

                Serde.Test.AllInOne newType = new Serde.Test.AllInOne()
                {BoolField = boolfield.Value, CharField = charfield.Value, ByteField = bytefield.Value, UShortField = ushortfield.Value, UIntField = uintfield.Value, ULongField = ulongfield.Value, SByteField = sbytefield.Value, ShortField = shortfield.Value, IntField = intfield.Value, LongField = longfield.Value, StringField = stringfield.Value, IntArr = intarr.Value, NestedArr = nestedarr.Value, IntImm = intimm.Value, };
                return newType;
            }
        }
    }
}