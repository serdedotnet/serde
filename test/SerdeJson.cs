
using System.Text;

namespace Serde
{
    public static class JsonSerializer
    {
        public static string ToString<T>(T value) where T : ISerialize
        {
            var builder = new StringBuilder();
            var serializer = new SerializerImpl(builder);
            value.Serialize<SerializerImpl, SerializerImpl>(serializer);
            return builder.ToString();
        }

        partial struct SerializerImpl
        {
            private readonly StringBuilder _builder;
            public SerializerImpl(StringBuilder builder)
            {
                _builder = builder;
            }
        }

        partial struct SerializerImpl : ISerializeType
        {
            public void End() => _builder.Append('}');

            private void CheckComma()
            {
                if (_builder[_builder.Length - 1] != '{')
                {
                    _builder.Append(',');
                }
            }

            void ISerializeType.SerializeField<T>(string name, T value)
            {
                CheckComma();
                Serialize(name);
                _builder.Append(':');
                value.Serialize<SerializerImpl, SerializerImpl>(this);
            }
        }

        partial struct SerializerImpl : ISerializer<SerializerImpl>
        {

            public void Serialize(bool b) => _builder.Append(b ? "true" : "false");

            public void Serialize(byte b) => _builder.Append(b);

            public void Serialize(ushort u16) => _builder.Append(u16);

            public void Serialize(uint u32) => _builder.Append(u32);

            public void Serialize(ulong u64) => _builder.Append(u64);

            public void Serialize(sbyte b) => _builder.Append(b);

            public void Serialize(short i16) => _builder.Append(i16);

            public void Serialize(int i32) => _builder.Append(i32);

            public void Serialize(long i64) => _builder.Append(i64);

            public void Serialize(string s)
            {
                _builder.Append('"');
                _builder.Append(s);
                _builder.Append('"');
            }

            public SerializerImpl SerializeType(string name, int numFields)
            {
                _builder.Append('{');
                return this;
            }
        }

    }
}