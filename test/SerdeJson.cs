
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

        partial struct SerializerImpl : ISerializeStruct
        {
            public void End() => _builder.Append('}');

            private void CheckComma()
            {
                if (_builder[_builder.Length - 1] != '{')
                {
                    _builder.Append(',');
                }
            }

            public void SerializeField(string name, bool b)
            {
                CheckComma();
                Serialize(name);
                _builder.Append(':');
                Serialize(b);
            }

            public void SerializeField(string name, byte b) => SerializeField(name, (int)b);

            public void SerializeField(string name, int i)
            {
                CheckComma();
                Serialize(name);
                _builder.Append(':');
                Serialize(i);
            }

            public void SerializeField(string name, string s)
            {
                CheckComma();
                Serialize(name);
                _builder.Append(':');
                Serialize(s);
            }

            public void SerializeField<T>(string name, T value) where T : ISerialize
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

            public void Serialize(int i) => _builder.Append(i);

            public void Serialize(string s) => _builder.Append(s);

            public SerializerImpl SerializeStruct(string name, int numFields)
            {
                _builder.Append('{');
                return this;
            }
        }

    }
}