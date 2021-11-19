
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]{"BoolField", "CharField", "ByteField", "UShortField", "UIntField", "ULongField", "SByteField", "ShortField", "IntField", "LongField", "StringField", "IntArr", "NestedArr", "IntImm"};
            return deserializer.DeserializeType<AllInOne, SerdeVisitor>("AllInOne", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne";
            Serde.Test.AllInOne Serde.IDeserializeVisitor<Serde.Test.AllInOne>.VisitDictionary<D>(ref D d)
            {
                Serde.Test.AllInOne newType = new Serde.Test.AllInOne();
                while (d.TryGetNextKey<string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case "BoolField":
                            newType.BoolField = d.GetNextValue<bool, BoolWrap>();
                            break;
                        case "CharField":
                            newType.CharField = d.GetNextValue<char, CharWrap>();
                            break;
                        case "ByteField":
                            newType.ByteField = d.GetNextValue<byte, ByteWrap>();
                            break;
                        case "UShortField":
                            newType.UShortField = d.GetNextValue<ushort, UInt16Wrap>();
                            break;
                        case "UIntField":
                            newType.UIntField = d.GetNextValue<uint, UInt32Wrap>();
                            break;
                        case "ULongField":
                            newType.ULongField = d.GetNextValue<ulong, UInt64Wrap>();
                            break;
                        case "SByteField":
                            newType.SByteField = d.GetNextValue<sbyte, SByteWrap>();
                            break;
                        case "ShortField":
                            newType.ShortField = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case "IntField":
                            newType.IntField = d.GetNextValue<int, Int32Wrap>();
                            break;
                        case "LongField":
                            newType.LongField = d.GetNextValue<long, Int64Wrap>();
                            break;
                        case "StringField":
                            newType.StringField = d.GetNextValue<string, StringWrap>();
                            break;
                        case "IntArr":
                            newType.IntArr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case "NestedArr":
                            newType.NestedArr = d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case "IntImm":
                            newType.IntImm = d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        default:
                            throw new InvalidDeserializeValueException("Unexpected field or property name in type Serde.Test.AllInOne: '" + key + "'");
                    }
                }

                return newType;
            }
        }
    }
}