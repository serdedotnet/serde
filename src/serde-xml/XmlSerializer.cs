
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
    public static string SerializeIndented<T>(T t) where T : ISerializeProvider<T>
        => Serialize(t, new XmlWriterSettings() { Indent = true });

    public static string Serialize<T>(T t) where T : ISerializeProvider<T>
        => Serialize(t, new XmlWriterSettings() { Indent = true });

    private static string Serialize<T>(T s, XmlWriterSettings? settings) where T : ISerializeProvider<T>
    {
        using var stringWriter = new StringWriter();
        using (var writer = XmlWriter.Create(stringWriter, settings))
        {
            var serializer = new XmlSerializer(writer);
            writer.WriteProcessingInstruction("xml", $"version=\"1.0\" encoding=\"{stringWriter.Encoding.WebName}\"");
            T.Instance.Serialize(s, serializer);
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
    public void WriteBool(bool b)
    {
        _writer.WriteValue(b);
    }

    public void WriteU8(byte b) => WriteI64(b);

    public void WriteChar(char c) => WriteString(c.ToString());

    public void WriteF64(double d)
    {
        _writer.WriteValue(d);
    }

    public void WriteF32(float f) => WriteF64(f);

    public void WriteI16(short i16) => WriteI64(i16);

    public void WriteI32(int i32) => WriteI64(i32);

    public void WriteI64(long i64)
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

    public void WriteNull()
    {
        // Default behavior is to skip serialization of null values
    }

    public void WriteI8(sbyte b) => WriteI64(b);

    public void WriteString(string s)
    {
        _writer.WriteString(s);
    }

    public void WriteU16(ushort u16) => WriteI64(u16);

    public void WriteU32(uint u32) => WriteI64(u32);

    public void WriteU64(ulong u64)
    {
        _writer.WriteValue((decimal)u64);
    }

    public void WriteDecimal(decimal d)
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

    sealed partial class SerializeCollectionImpl : ITypeSerializer
    {
        private readonly XmlSerializer _serializer;
        private readonly State _savedState;
        public SerializeCollectionImpl(XmlSerializer serializer, State savedState)
        {
            _serializer = serializer;
            _savedState = savedState;
        }

        void ITypeSerializer.End(ISerdeInfo typeInfo)
        {
            if (_savedState == State.Enumerable)
            {
                _serializer._writer.WriteEndElement();
            }
            _serializer._state = State.Enumerable;
        }

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b)
        {
            _serializer.WriteBool(b);
        }

        public void WriteChar(ISerdeInfo typeInfo, int index, char c)
        {
            _serializer.WriteChar(c);
        }

        public void WriteU8(ISerdeInfo typeInfo, int index, byte b)
        {
            _serializer.WriteU8(b);
        }

        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
        {
            _serializer.WriteU16(u16);
        }

        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32)
        {
            _serializer.WriteU32(u32);
        }

        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
        {
            _serializer.WriteU64(u64);
        }

        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
        {
            _serializer.WriteI8(b);
        }

        public void WriteI16(ISerdeInfo typeInfo, int index, short i16)
        {
            _serializer.WriteI16(i16);
        }

        public void WriteI32(ISerdeInfo typeInfo, int index, int i32)
        {
            _serializer.WriteI32(i32);
        }

        public void WriteI64(ISerdeInfo typeInfo, int index, long i64)
        {
            _serializer.WriteI64(i64);
        }

        public void WriteF32(ISerdeInfo typeInfo, int index, float f)
        {
            _serializer.WriteF32(f);
        }

        public void WriteF64(ISerdeInfo typeInfo, int index, double d)
        {
            _serializer.WriteF64(d);
        }

        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
        {
            _serializer.WriteDecimal(d);
        }

        public void WriteString(ISerdeInfo typeInfo, int index, string s)
        {
            _serializer.WriteString(s);
        }

        public void WriteNull(ISerdeInfo typeInfo, int index)
        {
            _serializer.WriteNull();
        }

        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class?
        {
            serialize.Serialize(value, _serializer);
        }
    }

    public ITypeSerializer WriteCollection(ISerdeInfo typeInfo, int? size)
    {
        switch (typeInfo.Kind)
        {
            case InfoKind.List:
            {
                var savedState = _state;
                if (savedState == State.Enumerable)
                {
                    _writer.WriteStartElement(FormatTypeName(typeInfo.Name));
                }
                _state = State.Enumerable;
                return new SerializeCollectionImpl(this, savedState);
            }
            case InfoKind.Dictionary:
                throw new NotSupportedException("Serde.Xml doesn't currently support Dictionary serialization.");
            default:
                throw new ArgumentException($"Expected List or Dictionary, found {typeInfo.Kind}");
        }
    }
    public ITypeSerializer WriteType(ISerdeInfo typeInfo)
    {
        switch (typeInfo.Kind)
        {
            case InfoKind.Enum:
                return new EnumSerializer(this);
            case InfoKind.Nullable:
            case InfoKind.CustomType:
            {
                var saved = _state;
                bool writeEnd;
                if (_state is State.Start or State.Enumerable)
                {
                    _writer.WriteStartElement(typeInfo.Name);
                    writeEnd = true;
                }
                else
                {
                    writeEnd = false;
                }
                _state = State.Type;
                return new XmlTypeSerializer(writeEnd, this, saved);
            }
            default:
                throw new NotSupportedException($"Serde.Xml doesn't currently support kind '{typeInfo.Kind}'");
        }
    }

    private sealed class EnumSerializer(XmlSerializer _parent) : ITypeSerializer
    {
        private void WriteEnumName(ISerdeInfo typeInfo, int index)
        {
            _parent._writer.WriteString(typeInfo.GetFieldStringName(index));
        }

        public void End(ISerdeInfo info) { }
        public void WriteBool(ISerdeInfo typeInfo, int index, bool b) => ThrowInvalidEnum();
        public void WriteU8(ISerdeInfo typeInfo, int index, byte b) => WriteEnumName(typeInfo, index);
        public void WriteChar(ISerdeInfo typeInfo, int index, char c) => ThrowInvalidEnum();
        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) => ThrowInvalidEnum();
        public void WriteF64(ISerdeInfo typeInfo, int index, double d) => ThrowInvalidEnum();
        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize) where T : class? => ThrowInvalidEnum();
        public void WriteF32(ISerdeInfo typeInfo, int index, float f) => ThrowInvalidEnum();
        public void WriteI16(ISerdeInfo typeInfo, int index, short i16) => WriteEnumName(typeInfo, index);
        public void WriteI32(ISerdeInfo typeInfo, int index, int i32) => WriteEnumName(typeInfo, index);
        public void WriteI64(ISerdeInfo typeInfo, int index, long i64) => WriteEnumName(typeInfo, index);
        public void WriteNull(ISerdeInfo typeInfo, int index) => ThrowInvalidEnum();
        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b) => WriteEnumName(typeInfo, index);
        public void WriteString(ISerdeInfo typeInfo, int index, string s) => ThrowInvalidEnum();
        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) => WriteEnumName(typeInfo, index);
        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32) => WriteEnumName(typeInfo, index);
        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64) => WriteEnumName(typeInfo, index);
        private void ThrowInvalidEnum() => throw new InvalidOperationException("Invalid operation for enum serialization, expected integer value.");
    }

    private sealed class XmlTypeSerializer : ITypeSerializer
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

        void ITypeSerializer.WriteValue<T>(ISerdeInfo typeInfo, int fieldIndex, T value, ISerialize<T> impl)
            => WriteField<T, ISerialize<T>>(typeInfo, fieldIndex, value, impl);

        private void WriteField<T, U>(ISerdeInfo typeInfo, int fieldIndex, T value, U impl)
            where U : ISerialize<T>
        {
            var name = typeInfo.GetFieldStringName(fieldIndex);
            foreach (var attr in typeInfo.GetFieldAttributes(fieldIndex))
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

        public void End(ISerdeInfo info)
        {
            if (_writeEnd)
            {
                _parent._writer.WriteEndElement();
            }
            _parent._state = _savedState;
        }

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b)
        {
            WriteField(typeInfo, index, b, BoolProxy.Instance);
        }

        public void WriteChar(ISerdeInfo typeInfo, int index, char c)
            => WriteField(typeInfo, index, c, CharProxy.Instance);

        public void WriteU8(ISerdeInfo typeInfo, int index, byte b)
            => WriteField(typeInfo, index, b, U8Proxy.Instance);

        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
            => WriteField(typeInfo, index, u16, U16Proxy.Instance);

        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32)
            => WriteField(typeInfo, index, u32, U32Proxy.Instance);

        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
            => WriteField(typeInfo, index, u64, U64Proxy.Instance);

        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
            => WriteField(typeInfo, index, b, I8Proxy.Instance);

        public void WriteI16(ISerdeInfo typeInfo, int index, short i16)
            => WriteField(typeInfo, index, i16, I16Proxy.Instance);

        public void WriteI32(ISerdeInfo typeInfo, int index, int i32)
            => WriteField(typeInfo, index, i32, I32Proxy.Instance);

        public void WriteI64(ISerdeInfo typeInfo, int index, long i64)
            => WriteField(typeInfo, index, i64, I64Proxy.Instance);

        public void WriteF32(ISerdeInfo typeInfo, int index, float f)
            => WriteField(typeInfo, index, f, F32Proxy.Instance);

        public void WriteF64(ISerdeInfo typeInfo, int index, double d)
            => WriteField(typeInfo, index, d, F64Proxy.Instance);

        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
            => WriteField(typeInfo, index, d, DecimalProxy.Instance);

        public void WriteString(ISerdeInfo typeInfo, int index, string s)
            => WriteField(typeInfo, index, s, StringProxy.Instance);

        public void WriteNull(ISerdeInfo typeInfo, int fieldIndex)
        {
            var name = typeInfo.GetFieldStringName(fieldIndex);
            foreach (var attr in typeInfo.GetFieldAttributes(fieldIndex))
            {
                if (attr.AttributeType == typeof(XmlAttributeAttribute))
                {
                    _parent._writer.WriteStartAttribute(name);
                    _parent._writer.WriteEndAttribute();
                    return;
                }
            }

            _parent._writer.WriteStartElement(name);
            _parent._writer.WriteEndElement();
        }
    }
}