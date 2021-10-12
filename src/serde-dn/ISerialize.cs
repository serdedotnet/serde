
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

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class GenerateWrapper : Attribute
    {
        public GenerateWrapper(string memberName) { }
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
        ISerializeType SerializeType(string name, int numFields);
        ISerializeEnumerable SerializeEnumerable(int? length);
        ISerializeDictionary SerializeDictionary(int? length);
    }
}