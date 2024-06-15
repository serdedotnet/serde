
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Serde;

public sealed partial class XmlSerializer
{
    /// <summary>
    /// Serialize the given type to a string.
    /// </summary>
    public static string SerializeIndented<T>(T t) where T : ISerialize<T>
        => Serialize(t, new XmlWriterSettings() { Indent = true });

    public static string Serialize<T>(T t) where T : ISerialize<T>
        => Serialize(t, new XmlWriterSettings() { Indent = true });

    private static string Serialize<T>(T s, XmlWriterSettings? settings) where T : ISerialize<T>
    {
        using var stringWriter = new StringWriter();
        using (var writer = XmlWriter.Create(stringWriter, settings))
        {
            var serializer = new XmlSerializer(writer);
            writer.WriteProcessingInstruction("xml", $"version=\"1.0\" encoding=\"{stringWriter.Encoding.WebName}\"");
            s.Serialize(s, serializer);
        }
        return stringWriter.ToString();
    }

    private readonly XmlWriter _writer;
    private State _state = State.Start;

    private enum State : byte
    {
        Start,
        // Serializing the fields of a type
        Type,
        // serializing the elements of an enumerable
        Enumerable
    }


    private XmlSerializer(XmlWriter writer)
    {
        _writer = writer;
    }
}

public sealed partial class XmlSerializer : ISerializer
{
    public void SerializeBool(bool b)
    {
        _writer.WriteValue(b);
    }

    public void SerializeByte(byte b) => SerializeI64(b);

    public void SerializeChar(char c) => SerializeString(c.ToString());

    public void SerializeDouble(double d)
    {
        _writer.WriteValue(d);
    }

    public void SerializeEnumValue<T>(string enumName, string? valueName, T value) where T : notnull, ISerialize
    {
    }

    void ISerializer.SerializeEnumValue<T, U>(string enumName, string? valueName, T value, U serialize)
    {}

    public void SerializeFloat(float f) => SerializeDouble(f);

    public void SerializeI16(short i16) => SerializeI64(i16);

    public void SerializeI32(int i32) => SerializeI64(i32);

    public void SerializeI64(long i64)
    {
        if (_state == State.Enumerable)
        {
            _writer.WriteStartElement("int");
        }
        _writer.WriteValue(i64);
        if (_state == State.Enumerable)
        {
            _writer.WriteEndElement();
        }
    }

    public void SerializeNotNull<T>(T t) where T : notnull, ISerialize
        => t.Serialize(this);

    void ISerializer.SerializeNotNull<T, U>(T t, U u)
        => u.Serialize(t, this);

    public void SerializeNull()
    {
        // Default behavior is to skip serialization of null values
    }

    public void SerializeSByte(sbyte b) => SerializeI64(b);

    public void SerializeString(string s)
    {
        _writer.WriteString(s);
    }

    public void SerializeU16(ushort u16) => SerializeI64(u16);

    public void SerializeU32(uint u32) => SerializeI64(u32);

    public void SerializeU64(ulong u64)
    {
        _writer.WriteValue((decimal)u64);
    }

    public void SerializeDecimal(decimal d)
    {
        _writer.WriteValue(d);
    }

    private sealed class ReflectionTypeNameFormattingListener : ReflectionTypeNameBaseListener
    {
        private readonly StringBuilder _buffer = new StringBuilder();
        public override void ExitTypeName([NotNull] ReflectionTypeNameParser.TypeNameContext context)
        {
            var brackets = context.BRACKETS();
            for (int i = 0; i < brackets.Length; i++)
            {
                _buffer.Append("ArrayOf");
            }
            var name = context.qualifiedName().children[^1];
            _buffer.Append(name);
        }
        public override void EnterGenerics([NotNull] ReflectionTypeNameParser.GenericsContext context)
        {
            _buffer.Append("Of");
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }

    private static string FormatTypeName(string name)
    {
        var stream = CharStreams.fromString(name);
        var lexer = new ReflectionTypeNameLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new ReflectionTypeNameParser(tokens);
        parser.BuildParseTree = true;
        var tree = parser.topTypeName();
        var formattingListener = new ReflectionTypeNameFormattingListener();
        ParseTreeWalker.Default.Walk(formattingListener, tree);
        return formattingListener.ToString();
    }

    public ISerializeEnumerable SerializeEnumerable(string typeName, int? length)
    {
        var savedState = _state;
        if (savedState == State.Enumerable)
        {
            _writer.WriteStartElement(FormatTypeName(typeName));
        }
        _state = State.Enumerable;
        return new SerializeEnumerableImpl(this, savedState);
    }

    sealed partial class SerializeEnumerableImpl : ISerializeEnumerable
    {
        private readonly XmlSerializer _serializer;
        private readonly State _savedState;
        public SerializeEnumerableImpl(XmlSerializer serializer, State savedState)
        {
            _serializer = serializer;
            _savedState = savedState;
        }

        void ISerializeEnumerable.SerializeElement<T>(T value)
        {
            value.Serialize(_serializer);
        }
        void ISerializeEnumerable.SerializeElement<T, U>(T value, U serialize)
            => serialize.Serialize(value, _serializer);

        void ISerializeEnumerable.End()
        {
            if (_savedState == State.Enumerable)
            {
                _serializer._writer.WriteEndElement();
            }
            _serializer._state = State.Enumerable;
        }
    }

    public ISerializeDictionary SerializeDictionary(int? length)
    {
        throw new NotSupportedException("Serde.XmlSerializer doesn't currently support serializing dictionaries");
    }

    public ISerializeType SerializeType(string name, int numFields)
    {
        var saved = _state;
        bool writeEnd;
        if (_state is State.Start or State.Enumerable)
        {
            _writer.WriteStartElement(name);
            writeEnd = true;
        }
        else
        {
            writeEnd = false;
        }
        _state = State.Type;
        return new XmlTypeSerializer(writeEnd, this, saved);
    }

    public ISerializeType SerializeType(TypeInfo typeInfo)
    {
        var saved = _state;
        bool writeEnd;
        if (_state is State.Start or State.Enumerable)
        {
            _writer.WriteStartElement(typeInfo.TypeName);
            writeEnd = true;
        }
        else
        {
            writeEnd = false;
        }
        _state = State.Type;
        return new XmlTypeSerializer(writeEnd, this, saved);
    }

    private sealed class XmlTypeSerializer : ISerializeType
    {
        private readonly bool _writeEnd;
        private readonly XmlSerializer _parent;
        private readonly State _savedState;

        public XmlTypeSerializer(bool writeEnd, XmlSerializer parent, State savedState)
        {
            _writeEnd = writeEnd;
            _parent = parent;
            _savedState = savedState;
        }

        public void SerializeField<T>(TypeInfo typeInfo, int fieldIndex, T value)
            where T : ISerialize<T>
            => SerializeField(typeInfo, fieldIndex, value, value);

        public void SerializeField<T, U>(TypeInfo typeInfo, int fieldIndex, T value)
            where U : struct, ISerialize<T>
            => SerializeField(typeInfo, fieldIndex, value, default(U));

        private void SerializeField<T, U>(TypeInfo typeInfo, int fieldIndex, T value, U impl)
            where U : ISerialize<T>
        {
            var name = typeInfo.GetStringSerializeName(fieldIndex);
            foreach (var attr in typeInfo.GetCustomAttributeData(fieldIndex))
            {
                if (attr.AttributeType == typeof(XmlAttributeAttribute))
                {
                    _parent._writer.WriteStartAttribute(name);
                    impl.Serialize(value, _parent);
                    _parent._writer.WriteEndAttribute();
                    return;
                }
            }

            _parent._writer.WriteStartElement(name);
            impl.Serialize(value, _parent);
            _parent._writer.WriteEndElement();
        }

        public void SerializeField<T>(string name, T value)
            where T : ISerialize
        {
            _parent._writer.WriteStartElement(name);
            value.Serialize(_parent);
            _parent._writer.WriteEndElement();
        }
        void ISerializeType.SerializeField<T>(ReadOnlySpan<byte> name, T value)
        {
            SerializeField(Encoding.UTF8.GetString(name), value);
        }

        public void SerializeField<T>(string name, T value, ReadOnlySpan<Attribute> attributes)
            where T : ISerialize
        {
            foreach (var attr in attributes)
            {
                if (attr is XmlAttributeAttribute)
                {
                    _parent._writer.WriteStartAttribute(name);
                    value.Serialize(_parent);
                    _parent._writer.WriteEndAttribute();
                    return;
                }
            }
            SerializeField(name, value);
        }

        public void SerializeField<T>(ReadOnlySpan<byte> name, T value, ReadOnlySpan<Attribute> attributes)
            where T : ISerialize
        {
            SerializeField(Encoding.UTF8.GetString(name), value, attributes);
        }

        void ISerializeType.SerializeField<T, U>(string name, T value)
        {
            _parent._writer.WriteStartElement(name);
            default(U).Serialize(value, _parent);
            _parent._writer.WriteEndElement();
        }

        public void End()
        {
            if (_writeEnd)
            {
                _parent._writer.WriteEndElement();
            }
            _parent._state = _savedState;
        }
    }
}