using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public class GenerateSerdeAttribute : Attribute { }

    public interface ISerialize
    {
        void Serialize<TSerializer, TSerializeType>(ref TSerializer serializer)
            where TSerializeType : ISerializeType
            where TSerializer : ISerializer<TSerializeType>;
    }

    public interface ISerializeType
    {
        void SerializeField<T>(string name, T value) where T : ISerialize;
        void End();
        void SkipField(string name) { }
    }

    public interface ISerializer<
        out TSerializeType
        >
        where TSerializeType : ISerializeType
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
    }
}
