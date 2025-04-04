// Contains implementations of data interfaces for core types

namespace Serde;

internal interface ISerdePrimitive<TSelf, T>
    : ISerde<T>, ISerdeProvider<TSelf, TSelf, T>, ITypeSerialize<T>, ITypeDeserialize<T>
    where TSelf : ISerdePrimitive<TSelf, T>
{
    /// <summary>
    /// Abstract static to force all primitives to provide a convenient static accessor.
    /// </summary>
    public new abstract static ISerdeInfo SerdeInfo { get; }
}

public sealed class BoolProxy : ISerdePrimitive<BoolProxy, bool>
{
    public static BoolProxy Instance { get; } = new();
    private BoolProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "bool";
    void ISerialize<bool>.Serialize(bool value, ISerializer serializer)
        => serializer.WriteBool(value);
    bool IDeserialize<bool>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadBool();
    void ITypeSerialize<bool>.Serialize(bool value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteBool(info, index, value);
    bool ITypeDeserialize<bool>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadBool(info, index);
}

public sealed class CharProxy : ISerdePrimitive<CharProxy, char>
{
    public static CharProxy Instance { get; } = new();
    private CharProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "char";
    void ISerialize<char>.Serialize(char value, ISerializer serializer)
        => serializer.WriteChar(value);
    char IDeserialize<char>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadChar();

    void ITypeSerialize<char>.Serialize(char value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteChar(info, index, value);

    char ITypeDeserialize<char>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadChar(info, index);
}

public sealed class U8Proxy : ISerdePrimitive<U8Proxy, byte>
{
    public static U8Proxy Instance { get; } = new();
    private U8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "byte";
    public void Serialize(byte value, ISerializer serializer)
        => serializer.WriteU8(value);
    public byte Deserialize(IDeserializer deserializer)
        => deserializer.ReadU8();

    void ITypeSerialize<byte>.Serialize(byte value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU8(info, index, value);

    byte ITypeDeserialize<byte>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU8(info, index);
}

public sealed class U16Proxy : ISerdePrimitive<U16Proxy, ushort>
{
    public static U16Proxy Instance { get; } = new();
    private U16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "ushort";
    void ISerialize<ushort>.Serialize(ushort value, ISerializer serializer)
        => serializer.WriteU16(value);
    ushort IDeserialize<ushort>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU16();

    ushort ITypeDeserialize<ushort>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU16(info, index);

    void ITypeSerialize<ushort>.Serialize(ushort value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU16(info, index, value);
}

public sealed class U32Proxy : ISerdePrimitive<U32Proxy, uint>
{
    public static U32Proxy Instance { get; } = new();
    private U32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "uint";
    void ISerialize<uint>.Serialize(uint value, ISerializer serializer)
        => serializer.WriteU32(value);
    uint IDeserialize<uint>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU32();

    uint ITypeDeserialize<uint>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU32(info, index);

    void ITypeSerialize<uint>.Serialize(uint value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU32(info, index, value);
}

public sealed class U64Proxy : ISerdePrimitive<U64Proxy, ulong>
{
    public static U64Proxy Instance { get; } = new();
    private U64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "ulong";
    void ISerialize<ulong>.Serialize(ulong value, ISerializer serializer)
        => serializer.WriteU64(value);
    ulong IDeserialize<ulong>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU64();

    void ITypeSerialize<ulong>.Serialize(ulong value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU64(info, index, value);

    ulong ITypeDeserialize<ulong>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU64(info, index);
}

public sealed class I8Proxy : ISerdePrimitive<I8Proxy, sbyte>
{
    public static I8Proxy Instance { get; } = new();
    private I8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "sbyte";
    void ISerialize<sbyte>.Serialize(sbyte value, ISerializer serializer)
        => serializer.WriteI8(value);
    sbyte IDeserialize<sbyte>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI8();

    void ITypeSerialize<sbyte>.Serialize(sbyte value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI8(info, index, value);

    sbyte ITypeDeserialize<sbyte>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI8(info, index);
}

public sealed class I16Proxy : ISerdePrimitive<I16Proxy, short>
{
    public static I16Proxy Instance { get; } = new();
    private I16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "short";
    void ISerialize<short>.Serialize(short value, ISerializer serializer)
        => serializer.WriteI16(value);
    short IDeserialize<short>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI16();

    void ITypeSerialize<short>.Serialize(short value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI16(info, index, value);

    short ITypeDeserialize<short>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI16(info, index);
}

public sealed class I32Proxy : ISerdePrimitive<I32Proxy, int>
{
    public static I32Proxy Instance { get; } = new();
    private I32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "int";
    void ISerialize<int>.Serialize(int value, ISerializer serializer)
        => serializer.WriteI32(value);
    int IDeserialize<int>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI32();

    void ITypeSerialize<int>.Serialize(int value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI32(info, index, value);

    int ITypeDeserialize<int>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI32(info, index);
}

public sealed class I64Proxy : ISerdePrimitive<I64Proxy, long>
{
    public static I64Proxy Instance { get; } = new();
    private I64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "long";
    void ISerialize<long>.Serialize(long value, ISerializer serializer)
        => serializer.WriteI64(value);
    long IDeserialize<long>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI64();

    void ITypeSerialize<long>.Serialize(long value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI64(info, index, value);

    long ITypeDeserialize<long>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI64(info, index);
}

public sealed class F32Proxy : ISerdePrimitive<F32Proxy, float>
{
    public static F32Proxy Instance { get; } = new();
    private F32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("float");
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    public void Serialize(float value, ISerializer serializer)
        => serializer.WriteF32(value);
    public float Deserialize(IDeserializer deserializer)
        => deserializer.ReadF32();

    void ITypeSerialize<float>.Serialize(float value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteF32(info, index, value);

    float ITypeDeserialize<float>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadF32(info, index);
}

public sealed class F64Proxy : ISerdePrimitive<F64Proxy, double>
{
    public static F64Proxy Instance { get; } = new();
    private F64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("double");
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<double>.Serialize(double value, ISerializer serializer)
        => serializer.WriteF64(value);
    double IDeserialize<double>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadF64();

    void ITypeSerialize<double>.Serialize(double value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteF64(info, index, value);

    double ITypeDeserialize<double>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadF64(info, index);
}

public sealed class DecimalProxy : ISerdePrimitive<DecimalProxy, decimal>
{
    public static DecimalProxy Instance { get; } = new();
    private DecimalProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("decimal");
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<decimal>.Serialize(decimal value, ISerializer serializer)
        => serializer.WriteDecimal(value);
    decimal IDeserialize<decimal>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDecimal();

    decimal ITypeDeserialize<decimal>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDecimal(info, index);

    void ITypeSerialize<decimal>.Serialize(decimal value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDecimal(info, index, value);
}

public sealed class StringProxy : ISerdePrimitive<StringProxy, string>
{
    public static StringProxy Instance { get; } = new();
    private StringProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "string";
    void ISerialize<string>.Serialize(string value, ISerializer serializer)
        => serializer.WriteString(value);
    public string Deserialize(IDeserializer deserializer)
        => deserializer.ReadString();

    string ITypeDeserialize<string>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadString(info, index);

    void ITypeSerialize<string>.Serialize(string value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteString(info, index, value);
}

public static class BoxProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<object?>, ITypeSerialize<T>
        where TProvider : ISerializeProvider<T>
    {
        private readonly ISerialize<T> _underlying = TProvider.Instance;

        public static readonly Ser<T, TProvider> Instance = new();

        private Ser() {}
        public ISerdeInfo SerdeInfo => _underlying.SerdeInfo;

        public void Serialize(object? value, ISerializer serializer)
        {
            _underlying.Serialize((T)value!, serializer);
        }
        public void Serialize(T value, ITypeSerializer serializer, ISerdeInfo info, int index)
        {
            serializer.WriteValue(info, index, value, this);
        }
    }

    public sealed class De<T, TProvider> : IDeserialize<object?>, ITypeDeserialize<T>
        where TProvider : IDeserializeProvider<T>
    {
        private IDeserialize<T> _underlying = TProvider.Instance;

        public static readonly De<T, TProvider> Instance = new();
        private De() {}

        public ISerdeInfo SerdeInfo => _underlying.SerdeInfo;
        public object? Deserialize(IDeserializer deserializer) => _underlying.Deserialize(deserializer);
        public T Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
            => (T)deserializer.ReadValue(info, index, this)!;
    }
}


public static class NullableProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<T?>, ISerializeProvider<T?>
        where T : struct
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly ISerialize<T> proxy = TProvider.Instance;
        private Ser() {}
        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is {} notnull)
            {
                proxy.Serialize(notnull, serializer);
            }
            else
            {
                serializer.WriteNull();
            }
        }
    }

    public sealed class De<T, TProvider> : IDeserialize<T?>, IDeserializeProvider<T?>
        where T : struct
        where TProvider : IDeserializeProvider<T>
    {
        public static De<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);
        private De() {}

        public T? Deserialize(IDeserializer deserializer)
        {
            return (T?)deserializer.ReadNullableRef<object>(BoxProxy.De<T, TProvider>.Instance!);
        }
    }
}

public static class NullableRefProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<T?>, ISerializeProvider<T?>
        where T : class?
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly ISerialize<T> _ser = TProvider.Instance;

        private Ser() {}

        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is null)
            {
                serializer.WriteNull();
            }
            else
            {
                _ser.Serialize(value, serializer);
            }
        }
    }

    public sealed class De<T, TProvider> : IDeserialize<T?>, IDeserializeProvider<T?>
        where T : class
        where TProvider : IDeserializeProvider<T>
    {
        public static De<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly IDeserialize<T> _de = TProvider.Instance;

        private De() { }

        public T? Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef(_de);
        }
    }
}
