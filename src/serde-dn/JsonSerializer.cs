
using System.IO;

namespace Serde
{
    public sealed partial class JsonSerializer
    {
        // Simple helpers for the most common uses
        public static string WriteToString<T>(T t) where T : ISerialize
        {
            var writer = new StringWriter();
            var serializer = new Impl(writer);
            t.Serialize<Impl, Impl>(serializer);
            return writer.ToString();
        }

        // Using a mutable struct allows for an efficient low-allocation implementation of the
        // ISerializer interface, but mutable structs are easy to misuse in C#, so hide the
        // implementation for now.
        private partial struct Impl
        {
            private readonly TextWriter _writer;
            private TypeState _state;
            public Impl(TextWriter writer)
            {
                _writer = writer;
                _state = TypeState.Start;
            }
        }
    }

    // Implementations of ISerializer interfaces
    partial class JsonSerializer
    {
        partial struct Impl : ISerializer<Impl>
        {
            public void Serialize(bool b) => _writer.Write(b ? "true" : "false");

            public void Serialize(char c) => Serialize(c.ToString());

            public void Serialize(byte b) => _writer.Write(b);

            public void Serialize(ushort u16) => _writer.Write(u16);

            public void Serialize(uint u32) => _writer.Write(u32);

            public void Serialize(ulong u64) => _writer.Write(u64);

            public void Serialize(sbyte b) => _writer.Write(b);

            public void Serialize(short i16) => _writer.Write(i16);

            public void Serialize(int i32) => _writer.Write(i32);

            public void Serialize(long i64) => _writer.Write(i64);

            public void Serialize(string s)
            {
                _writer.Write('"');
                _writer.Write(s);
                _writer.Write('"');
            }
        }

        partial struct Impl : ISerializeType
        {
            private enum TypeState
            {
                Start = 0,
                WritingFields,
                End
            }
            public Impl SerializeType(string name, int numFields)
            {
                _state = TypeState.Start;
                _writer.Write('{');
                return this;
            }

            public void SerializeField<T>(string name, T value)
                where T : ISerialize
            {
                switch (_state)
                {
                    case TypeState.WritingFields:
                        _writer.Write(',');
                        goto case TypeState.Start;

                    case TypeState.Start:
                        this.Serialize(name);
                        _writer.Write(':');
                        value.Serialize<Impl, Impl>(this);
                        _state = TypeState.WritingFields;
                        break;
                }
            }

            public void End()
            {
                _writer.Write('}');
                _state = TypeState.End;
            }
        }
    }
}