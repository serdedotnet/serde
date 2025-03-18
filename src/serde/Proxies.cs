// Contains implementations of data interfaces for core types

namespace Serde;

public sealed class BoolProxy
    : ISerialize<bool>, IDeserialize<bool>,
      ISerializeProvider<bool>, IDeserializeProvider<bool>,
      ITypeSerialize<bool>, ITypeDeserialize<bool>
{
    public static BoolProxy Instance { get; } = new();
    static ISerialize<bool> ISerializeProvider<bool>.SerializeInstance => Instance;
    static IDeserialize<bool> IDeserializeProvider<bool>.DeserializeInstance => Instance;

    private BoolProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class CharProxy
    : ISerialize<char>, IDeserialize<char>,
    ISerializeProvider<char>, IDeserializeProvider<char>,
    ITypeSerialize<char>, ITypeDeserialize<char>
{
    public static CharProxy Instance { get; } = new();
    static ISerialize<char> ISerializeProvider<char>.SerializeInstance => Instance;
    static IDeserialize<char> IDeserializeProvider<char>.DeserializeInstance => Instance;
    private CharProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class U8Proxy
    : ISerialize<byte>, IDeserialize<byte>,
      ISerializeProvider<byte>, IDeserializeProvider<byte>,
    ITypeSerialize<byte>, ITypeDeserialize<byte>
{
    public static U8Proxy Instance { get; } = new();
    static ISerialize<byte> ISerializeProvider<byte>.SerializeInstance => Instance;
    static IDeserialize<byte> IDeserializeProvider<byte>.DeserializeInstance => Instance;
    private U8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class U16Proxy
    : ISerialize<ushort>, IDeserialize<ushort>,
    ISerializeProvider<ushort>, IDeserializeProvider<ushort>,
    ITypeSerialize<ushort>, ITypeDeserialize<ushort>
{
    public static U16Proxy Instance { get; } = new();
    static ISerialize<ushort> ISerializeProvider<ushort>.SerializeInstance => Instance;
    static IDeserialize<ushort> IDeserializeProvider<ushort>.DeserializeInstance => Instance;
    private U16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class U32Proxy
    : ISerialize<uint>, IDeserialize<uint>,
      ISerializeProvider<uint>, IDeserializeProvider<uint>,
      ITypeSerialize<uint>, ITypeDeserialize<uint>
{
    public static U32Proxy Instance { get; } = new();
    static ISerialize<uint> ISerializeProvider<uint>.SerializeInstance => Instance;
    static IDeserialize<uint> IDeserializeProvider<uint>.DeserializeInstance => Instance;
    private U32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class U64Proxy
    : ISerialize<ulong>, IDeserialize<ulong>,
    ISerializeProvider<ulong>, IDeserializeProvider<ulong>,
    ITypeSerialize<ulong>, ITypeDeserialize<ulong>
{
    public static U64Proxy Instance { get; } = new();
    static ISerialize<ulong> ISerializeProvider<ulong>.SerializeInstance => Instance;
    static IDeserialize<ulong> IDeserializeProvider<ulong>.DeserializeInstance => Instance;
    private U64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class I8Proxy
    : ISerialize<sbyte>, IDeserialize<sbyte>,
    ISerializeProvider<sbyte>, IDeserializeProvider<sbyte>,
    ITypeSerialize<sbyte>, ITypeDeserialize<sbyte>
{
    public static I8Proxy Instance { get; } = new();
    static ISerialize<sbyte> ISerializeProvider<sbyte>.SerializeInstance => Instance;
    static IDeserialize<sbyte> IDeserializeProvider<sbyte>.DeserializeInstance => Instance;
    private I8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class I16Proxy
    : ISerialize<short>, IDeserialize<short>,
    ISerializeProvider<short>, IDeserializeProvider<short>,
    ITypeSerialize<short>, ITypeDeserialize<short>
{
    public static I16Proxy Instance { get; } = new();
    static ISerialize<short> ISerializeProvider<short>.SerializeInstance => Instance;
    static IDeserialize<short> IDeserializeProvider<short>.DeserializeInstance => Instance;
    private I16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class I32Proxy
    : ISerialize<int>, IDeserialize<int>,
      ISerializeProvider<int>, IDeserializeProvider<int>,
      ITypeSerialize<int>, ITypeDeserialize<int>
{
    public static I32Proxy Instance { get; } = new();
    static ISerialize<int> ISerializeProvider<int>.SerializeInstance => Instance;
    static IDeserialize<int> IDeserializeProvider<int>.DeserializeInstance => Instance;
    private I32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class I64Proxy
    : ISerialize<long>, IDeserialize<long>,
    ISerializeProvider<long>, IDeserializeProvider<long>,
    ITypeSerialize<long>, ITypeDeserialize<long>
{
    public static I64Proxy Instance { get; } = new();
    static ISerialize<long> ISerializeProvider<long>.SerializeInstance => Instance;
    static IDeserialize<long> IDeserializeProvider<long>.DeserializeInstance => Instance;
    private I64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

public sealed class F32Proxy : ISerialize<float>, IDeserialize<float>,
    ISerializeProvider<float>, IDeserializeProvider<float>,
    ITypeSerialize<float>, ITypeDeserialize<float>
{
    public static F32Proxy Instance { get; } = new();
    static ISerialize<float> ISerializeProvider<float>.SerializeInstance => Instance;
    static IDeserialize<float> IDeserializeProvider<float>.DeserializeInstance => Instance;
    private F32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("float");

    public void Serialize(float value, ISerializer serializer)
        => serializer.WriteF32(value);
    public float Deserialize(IDeserializer deserializer)
        => deserializer.ReadF32();

    void ITypeSerialize<float>.Serialize(float value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteF32(info, index, value);

    float ITypeDeserialize<float>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadF32(info, index);
}

public sealed class F64Proxy
    : ISerialize<double>, IDeserialize<double>,
    ISerializeProvider<double>, IDeserializeProvider<double>,
    ITypeSerialize<double>, ITypeDeserialize<double>
{
    public static F64Proxy Instance { get; } = new();
    static ISerialize<double> ISerializeProvider<double>.SerializeInstance => Instance;
    static IDeserialize<double> IDeserializeProvider<double>.DeserializeInstance => Instance;
    private F64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("double");

    void ISerialize<double>.Serialize(double value, ISerializer serializer)
        => serializer.WriteF64(value);
    double IDeserialize<double>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadF64();

    void ITypeSerialize<double>.Serialize(double value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteF64(info, index, value);

    double ITypeDeserialize<double>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadF64(info, index);
}

public sealed class DecimalProxy
    : ISerialize<decimal>, IDeserialize<decimal>,
      ISerializeProvider<decimal>, IDeserializeProvider<decimal>,
      ITypeSerialize<decimal>, ITypeDeserialize<decimal>
{
    public static DecimalProxy Instance { get; } = new();
    static ISerialize<decimal> ISerializeProvider<decimal>.SerializeInstance => Instance;
    static IDeserialize<decimal> IDeserializeProvider<decimal>.DeserializeInstance => Instance;
    private DecimalProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("decimal");

    void ISerialize<decimal>.Serialize(decimal value, ISerializer serializer)
        => serializer.WriteDecimal(value);
    decimal IDeserialize<decimal>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDecimal();

    decimal ITypeDeserialize<decimal>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDecimal(info, index);

    void ITypeSerialize<decimal>.Serialize(decimal value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDecimal(info, index, value);
}

public sealed class StringProxy
    : ISerialize<string>, IDeserialize<string>,
      ISerializeProvider<string>, IDeserializeProvider<string>,
      ITypeDeserialize<string>, ITypeSerialize<string>
{
    public static StringProxy Instance { get; } = new();
    static ISerialize<string> ISerializeProvider<string>.SerializeInstance => Instance;
    static IDeserialize<string> IDeserializeProvider<string>.DeserializeInstance => Instance;
    private StringProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

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

/// <summary>
/// We use a separate class to store the nullable info instance to provide reference equality
/// between the serialize and deserialize <see cref="ISerdeInfoProvider" /> implementations.
/// </summary>
file static class NullableInfoCache<TProvider>
    where TProvider : ISerdeInfoProvider
{
    public static readonly ISerdeInfo Instance = SerdeInfo.MakeNullable(TProvider.SerdeInfo);
}

public static class NullableProxy
{
    public sealed class Ser<T, TProvider> :ISerialize<T?>, ISerializeProvider<T?>
        where T : struct
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;

        private readonly ISerialize<T> proxy = TProvider.SerializeInstance;
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
        static IDeserialize<T?> IDeserializeProvider<T?>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;
        private De() {}
        private readonly BoxProxy _boxProxy = new(TProvider.DeserializeInstance);

        public T? Deserialize(IDeserializer deserializer)
        {
            return (T?)deserializer.ReadNullableRef(_boxProxy);
        }

        private sealed class BoxProxy(IDeserialize<T> underlyingProxy) : IDeserialize<object>
        {
            public object Deserialize(IDeserializer deserializer)
            {
                return underlyingProxy.Deserialize(deserializer);
            }
        }
    }
}

public static class NullableRefProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<T?>, ISerializeProvider<T?>
        where T : class
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;

        private readonly ISerialize<T> _ser = TProvider.SerializeInstance;

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
        static IDeserialize<T?> IDeserializeProvider<T?>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;

        private readonly IDeserialize<T> _de = TProvider.DeserializeInstance;

        private De() { }

        public T? Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef(_de);
        }
    }
}

/// <summary>
/// This is a perf optimization. It allows primitive types (and only primitive types) to be
/// serialized without boxing. It is only useful for serializing collections.
/// </summary>
public interface ITypeSerialize<T>
{
    void Serialize(T value, ITypeSerializer serializer, ISerdeInfo info, int index);
}

public sealed class TypeSerBoxed<T>(ISerialize<T> s) : ITypeSerialize<T>
{
    private sealed class BoxProxy(ISerialize<T> underlying) : ISerialize<object?>
    {
        public void Serialize(object? value, ISerializer serializer)
        {
            underlying.Serialize((T)value!, serializer);
        }
    }
    private readonly BoxProxy _boxProxy = new(s);

    public void Serialize(T value, ITypeSerializer serializer, ISerdeInfo info, int index)
    {
        serializer.WriteValue(info, index, value, _boxProxy);
    }
}

/// <summary>
/// This is a perf optimization. It allows primitive types (and only primitive types) to be
/// deserialized without boxing. It is only useful for deserializing collections.
/// </summary>
public interface ITypeDeserialize<T>
{
    T Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index);
}

public sealed class TypeDeBoxed<T>(IDeserialize<T> d) : ITypeDeserialize<T>
{
    private sealed class BoxProxy(IDeserialize<T> underlying) : IDeserialize<object?>
    {
        public object? Deserialize(IDeserializer deserializer)
        {
            return underlying.Deserialize(deserializer);
        }
    }
    private readonly BoxProxy _boxProxy = new(d);

    T ITypeDeserialize<T>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
    {
        return (T)deserializer.ReadValue(info, index, _boxProxy)!;
    }
}