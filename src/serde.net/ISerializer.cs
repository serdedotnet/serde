using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,
        AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public class GenerateSerdeAttribute : Attribute { }

    public interface ISerialize
    {
        void Serialize<TSerializer,TSerializeStruct>(TSerializer serializer)
            where TSerializeStruct : ISerializeStruct
            where TSerializer : ISerializer<TSerializeStruct>;
    }

    public interface ISerializeStruct
    {
        void SerializeField(string name, bool b);
        void SerializeField(string name, byte b);
        void SerializeField(string name, int i);
        void SerializeField(string name, string s);
        void SerializeField<T>(string name, T value) where T : ISerialize;
        void End();
        void SkipField(string name) { }
    }

    public interface ISerializer<
        TSerializeStruct>
        where TSerializeStruct : ISerializeStruct
    {
        void Serialize(bool b);
        void Serialize(byte b);
        void Serialize(int i);
        void Serialize(string s);
        TSerializeStruct SerializeStruct(string name, int numFields);
    }
}
