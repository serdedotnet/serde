
using System;
using System.IO;

namespace Serde
{
    public static partial class JsonSerializer
    {
        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string WriteToString<T>(T s) where T : ISerialize
        {
            var writer = new StringWriter();
            var serializer = new Impl(writer);
            s.Serialize<Impl, SerializeType>(ref serializer);
            return writer.ToString();
        }

        // Using a mutable struct allows for an efficient low-allocation implementation of the
        // ISerializer interface, but mutable structs are easy to misuse in C#, so hide the
        // implementation for now.
        private partial struct Impl
        {
            private readonly TextWriter _writer;
            public Impl(TextWriter writer)
            {
                _writer = writer;
            }

            public void Write(char c) => _writer.Write(c);
        }
    }

    // Implementations of ISerializer interfaces
    partial class JsonSerializer
    {
        partial struct Impl : ISerializer<SerializeType>, ISerializer<ISerializeType>
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

            public SerializeType SerializeType(string name, int numFields)
            {
                _writer.Write('{');
                // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
                // reference type, but that works for now.
                return new SerializeType(this);
            }

            ISerializeType ISerializer<ISerializeType>.SerializeType(string name, int numFields)
                => SerializeType(name, numFields);
        }

        struct SerializeType : ISerializeType
        {
            private enum State
            {
                Start = 0,
                WritingFields,
                End
            }
            private State _state;
            private Impl _impl;
            public SerializeType(Impl impl)
            {
                _state = State.Start;
                _impl = impl;
            }

            public void SerializeField<T>(string name, T value)
                where T : ISerialize
            {
                switch (_state)
                {
                    case State.Start:
                        _state = State.WritingFields;
                        break;

                    case State.WritingFields:
                        _impl.Write(',');
                        break;

                    case State.End:
                        throw new InvalidOperationException("Cannot write fields after calling End()");
                }

                _impl.Serialize(name);
                _impl.Write(':');
                value.Serialize<Impl, SerializeType>(ref _impl);
            }

            public void End()
            {
                _impl.Write('}');
                _state = State.End;
            }
        }
    }
}