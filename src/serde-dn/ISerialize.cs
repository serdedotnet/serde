namespace Serde
{
    public interface ISerialize
    {
        void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>;
    }

    public interface ISerializeType
    {
        void SerializeField<T>(string name, T value) where T : ISerialize;
        void End();
        void SkipField(string name) { }
    }

    public interface ISerializeEnumerable
    {
        void SerializeElement<T>(T value) where T : ISerialize;
        void End();
    }

    public interface ISerializeDictionary
    {
        void SerializeKey<T>(T key) where T : ISerialize;
        void SerializeValue<T>(T value) where T : ISerialize;
        void End();
    }

    public interface ISerializer<
        out TSerializeType,
        out TSerializeEnumerable,
        out TSerializeDictionary
        >
        where TSerializeType : ISerializeType
        where TSerializeEnumerable : ISerializeEnumerable
        where TSerializeDictionary : ISerializeDictionary
    {
        void SerializeBool(bool b);
        void SerializeChar(char c);
        void SerializeByte(byte b);
        void SerializeU16(ushort u16);
        void SerializeU32(uint u32);
        void SerializeU64(ulong u64);
        void SerializeSByte(sbyte b);
        void SerializeI16(short i16);
        void SerializeI32(int i32);
        void SerializeI64(long i64);
        void SerializeFloat(float f);
        void SerializeDouble(double d);
        void SerializeString(string s);
        TSerializeType SerializeType(string name, int numFields);
        TSerializeEnumerable SerializeEnumerable(int? length);
        TSerializeDictionary SerializeDictionary(int? length);
    }
}