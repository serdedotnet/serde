using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public class GenerateSerdeAttribute : Attribute { }

    public interface ISerializeShared
    {
        void Serialize(ISerializer<ISerializeType, ISerializeEnumerable> serializer);
    }

    public interface ISerialize : ISerializeShared
    {
        void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>;

        void ISerializeShared.Serialize(ISerializer<ISerializeType, ISerializeEnumerable> serializer)
            => Serialize<ISerializer<ISerializeType, ISerializeEnumerable>, ISerializeType, ISerializeEnumerable>(ref serializer);
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

    public interface ISerializer<
        out TSerializeType,
        out TSerializeEnumerable
        >
        where TSerializeType : ISerializeType
        where TSerializeEnumerable : ISerializeEnumerable
    {
        void Serialize(bool b);
        void Serialize(char c);
        void Serialize(byte b);
        void Serialize(ushort u16);
        void Serialize(uint u32);
        void Serialize(ulong u64);
        void Serialize(sbyte b);
        void Serialize(short i16);
        void Serialize(int i32);
        void Serialize(long i64);
        void Serialize(string s);
        TSerializeType SerializeType(string name, int numFields);
        TSerializeEnumerable SerializeEnumerable(int? length);
    }
}
