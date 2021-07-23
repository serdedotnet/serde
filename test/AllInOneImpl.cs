
namespace Serde.Test
{
    public partial class AllInOne : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            var type = serializer.SerializeType("AllInOne", 13);
            type.SerializeField("BoolField", new BoolWrap(BoolField));
            type.SerializeField("CharField", new CharWrap(CharField));
            type.SerializeField("ByteField", new ByteWrap(ByteField));
            type.SerializeField("UShortField", new UInt16Wrap(UShortField));
            type.SerializeField("UIntField", new UInt32Wrap(UIntField));
            type.SerializeField("ULongField", new UInt64Wrap(ULongField));
            type.SerializeField("SByteField", new SByteWrap(SByteField));
            type.SerializeField("ShortField", new Int16Wrap(ShortField));
            type.SerializeField("IntField", new Int32Wrap(IntField));
            type.SerializeField("LongField", new Int64Wrap(LongField));
            type.SerializeField("StringField", new StringWrap(StringField));
            type.SerializeField("IntArr", new ArrayWrap<int, Int32Wrap>(IntArr));
            type.SerializeField("NestedArr", new ArrayWrap<int[], ArrayWrap<int, Int32Wrap>>(NestedArr));
            type.SerializeField("IntImm", new ImmutableArrayWrap<int, Int32Wrap>(IntImm));
            type.End();
        }
    }
}
