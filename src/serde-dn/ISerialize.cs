
using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateSerializeAttribute : Attribute
    {
        /// <summary>
        /// Whether or not to generate an implementation for ISerializeStatic.
        /// Currently always false, as the generator does not support it yet.
        /// </summary>
        public bool Static { get; } = false;
    }

    public interface ISerialize
    {
        void Serialize(ISerializer serializer);
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

    public interface ISerializer
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
        void Serialize(float f);
        void Serialize(double d);
        void Serialize(string s);
        ISerializeType SerializeType(string name, int numFields);
        ISerializeEnumerable SerializeEnumerable(int? length);
        ISerializeDictionary SerializeDictionary(int? length);
    }
}