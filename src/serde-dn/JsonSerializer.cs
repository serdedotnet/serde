
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
            s.Serialize<Impl, SerializeType, SerializeEnumerable>(ref serializer);
            return writer.ToString();
        }

        public static string WriteToString(ISerializeShared s)
        {
            var writer = new StringWriter();
            var serializer = new Impl(writer);
            s.Serialize(serializer);
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
        partial struct Impl : ISerializer<SerializeType, SerializeEnumerable>, ISerializer<ISerializeType, ISerializeEnumerable>
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

            public void Serialize(float f) => _writer.Write(f);

            public void Serialize(double d) => _writer.Write(d);

            public void Serialize(string s)
            {
                _writer.Write('"');
                _writer.Write(s);
                _writer.Write('"');
            }

            public SerializeType SerializeType(string name, int numFields)
            {
                _writer.Write('{');
                return new SerializeType(ref this);
            }

            public SerializeEnumerable SerializeEnumerable(int? count)
            {
                _writer.Write('[');
                return new SerializeEnumerable(ref this);
            }

            ISerializeType ISerializer<ISerializeType, ISerializeEnumerable>.SerializeType(string name, int numFields)
                => SerializeType(name, numFields);

            ISerializeEnumerable ISerializer<ISerializeType, ISerializeEnumerable>.SerializeEnumerable(int? count)
                => SerializeEnumerable(count);
        }

        struct SerializeType : ISerializeType
        {
            private enum State : byte
            {
                Start = 0,
                WritingFields,
                End
            }
            private State _state;
            private Impl _impl;
            public SerializeType(ref Impl impl)
            {
                _state = State.Start;
                // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
                // reference type, but that works for now.
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
                value.Serialize<Impl, SerializeType, SerializeEnumerable>(ref _impl);
            }

            public void End()
            {
                _impl.Write('}');
                _state = State.End;
            }
        }

        struct SerializeEnumerable : ISerializeEnumerable
        {
            private enum State : byte
            {
                Start = 0,
                WritingElements,
                End
            }
            private State _state;
            private Impl _impl;
            public SerializeEnumerable(ref Impl impl)
            {
                _state = State.Start;
                // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
                // reference type, but that works for now.
                _impl = impl;
            }
            void ISerializeEnumerable.SerializeElement<T>(T value)
            {
                switch (_state)
                {
                    case State.Start:
                        _state = State.WritingElements;
                        break;

                    case State.WritingElements:
                        _impl.Write(',');
                        break;

                    case State.End:
                        throw new InvalidOperationException("Cannot write fields after calling End()");
                }
                value.Serialize<Impl, SerializeType, SerializeEnumerable>(ref _impl);
            }

            void ISerializeEnumerable.End()
            {
                _impl.Write(']');
                _state = State.End;
            }
        }
    }
}