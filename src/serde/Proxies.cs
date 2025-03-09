// Contains implementations of data interfaces for core types

namespace Serde;

public sealed class BoolProxy
    : ISerialize<bool>, IDeserialize<bool>,
      ISerializeProvider<bool>, IDeserializeProvider<bool>
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
}

public sealed class CharProxy
    : ISerialize<char>, IDeserialize<char>,
    ISerializeProvider<char>, IDeserializeProvider<char>
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
    {
        return deserializer.ReadChar();
    }
}

public sealed class ByteProxy
    : ISerialize<byte>, IDeserialize<byte>,
      ISerializeProvider<byte>, IDeserializeProvider<byte>
{
    public static ByteProxy Instance { get; } = new();
    static ISerialize<byte> ISerializeProvider<byte>.SerializeInstance => Instance;
    static IDeserialize<byte> IDeserializeProvider<byte>.DeserializeInstance => Instance;
    private ByteProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "byte";
    public void Serialize(byte value, ISerializer serializer)
    {
        serializer.WriteByte(value);
    }
    public byte Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadByte();
    }
}

public sealed class U16Proxy
    : ISerialize<ushort>, IDeserialize<ushort>,
    ISerializeProvider<ushort>, IDeserializeProvider<ushort>
{
    public static U16Proxy Instance { get; } = new();
    static ISerialize<ushort> ISerializeProvider<ushort>.SerializeInstance => Instance;
    static IDeserialize<ushort> IDeserializeProvider<ushort>.DeserializeInstance => Instance;
    private U16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "ushort";
    void ISerialize<ushort>.Serialize(ushort value, ISerializer serializer)
    {
        serializer.WriteU16(value);
    }
    ushort IDeserialize<ushort>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadU16();
    }
}

public sealed class U32Proxy
    : ISerialize<uint>, IDeserialize<uint>,
      ISerializeProvider<uint>, IDeserializeProvider<uint>
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
    {
        return deserializer.ReadU32();
    }
}

public sealed class U64Proxy
    : ISerialize<ulong>, IDeserialize<ulong>,
    ISerializeProvider<ulong>, IDeserializeProvider<ulong>
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
    {
        return deserializer.ReadU64();
    }
}

public sealed class SByteProxy
    : ISerialize<sbyte>, IDeserialize<sbyte>,
    ISerializeProvider<sbyte>, IDeserializeProvider<sbyte>
{
    public static SByteProxy Instance { get; } = new();
    static ISerialize<sbyte> ISerializeProvider<sbyte>.SerializeInstance => Instance;
    static IDeserialize<sbyte> IDeserializeProvider<sbyte>.DeserializeInstance => Instance;
    private SByteProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "sbyte";
    void ISerialize<sbyte>.Serialize(sbyte value, ISerializer serializer)
        => serializer.WriteSByte(value);
    sbyte IDeserialize<sbyte>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadSByte();
    }
}

public sealed class I16Proxy
    : ISerialize<short>, IDeserialize<short>,
    ISerializeProvider<short>, IDeserializeProvider<short>
{
    public static I16Proxy Instance { get; } = new();
    static ISerialize<short> ISerializeProvider<short>.SerializeInstance => Instance;
    static IDeserialize<short> IDeserializeProvider<short>.DeserializeInstance => Instance;
    private I16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "short";
    void ISerialize<short>.Serialize(short value, ISerializer serializer)
    {
        serializer.WriteI16(value);
    }
    short IDeserialize<short>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadI16();
    }
}

public sealed class I32Proxy
    : ISerialize<int>, IDeserialize<int>,
      ISerializeProvider<int>, IDeserializeProvider<int>
{
    public static I32Proxy Instance { get; } = new();
    static ISerialize<int> ISerializeProvider<int>.SerializeInstance => Instance;
    static IDeserialize<int> IDeserializeProvider<int>.DeserializeInstance => Instance;
    private I32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "int";
    void ISerialize<int>.Serialize(int value, ISerializer serializer)
    {
        serializer.WriteI32(value);
    }
    int IDeserialize<int>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadI32();
    }
}

public sealed class I64Proxy
    : ISerialize<long>, IDeserialize<long>,
    ISerializeProvider<long>, IDeserializeProvider<long>
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
    {
        return deserializer.ReadI64();
    }
}

public sealed class SingleProxy : ISerialize<float>, IDeserialize<float>,
    ISerializeProvider<float>, IDeserializeProvider<float>
{
    public static SingleProxy Instance { get; } = new();
    static ISerialize<float> ISerializeProvider<float>.SerializeInstance => Instance;
    static IDeserialize<float> IDeserializeProvider<float>.DeserializeInstance => Instance;
    private SingleProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("float");

    public void Serialize(float value, ISerializer serializer)
        => serializer.WriteFloat(value);
    public float Deserialize(IDeserializer deserializer)
        => deserializer.ReadFloat();
}

public sealed class DoubleProxy
    : ISerialize<double>, IDeserialize<double>,
    ISerializeProvider<double>, IDeserializeProvider<double>
{
    public static DoubleProxy Instance { get; } = new();
    static ISerialize<double> ISerializeProvider<double>.SerializeInstance => Instance;
    static IDeserialize<double> IDeserializeProvider<double>.DeserializeInstance => Instance;
    private DoubleProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("double");

    void ISerialize<double>.Serialize(double value, ISerializer serializer)
    {
        serializer.WriteDouble(value);
    }
    double IDeserialize<double>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadDouble();
    }
}

public sealed class DecimalProxy
    : ISerialize<decimal>, IDeserialize<decimal>,
    ISerializeProvider<decimal>, IDeserializeProvider<decimal>
{
    public static DecimalProxy Instance { get; } = new();
    static ISerialize<decimal> ISerializeProvider<decimal>.SerializeInstance => Instance;
    static IDeserialize<decimal> IDeserializeProvider<decimal>.DeserializeInstance => Instance;
    private DecimalProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("decimal");

    void ISerialize<decimal>.Serialize(decimal value, ISerializer serializer)
    {
        serializer.WriteDecimal(value);
    }
    decimal IDeserialize<decimal>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadDecimal();
    }
}

public sealed class StringProxy
    : ISerialize<string>, IDeserialize<string>,
      ISerializeProvider<string>, IDeserializeProvider<string>
{
    public static StringProxy Instance { get; } = new();
    static ISerialize<string> ISerializeProvider<string>.SerializeInstance => Instance;
    static IDeserialize<string> IDeserializeProvider<string>.DeserializeInstance => Instance;
    private StringProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "string";
    void ISerialize<string>.Serialize(string value, ISerializer serializer)
    {
        serializer.WriteString(value);
    }
    public string Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadString();
    }
}

/// <summary>
/// We use a separate class to store the nullable info instance to provide reference equality
/// between the serialize and deserialize <see cref="ISerdeInfoProvider" /> implementations.
/// </summary>
file static class NullableInfoCache<TProvider>
    where TProvider : ISerdeInfoProvider
{
    public static readonly ISerdeInfo Instance = Serde.SerdeInfo.MakeNullable(TProvider.SerdeInfo);
}

public static class NullableProxy
{
    public sealed class Serialize<T, TProvider> : SerializeInstance<T, ISerialize<T>>, ISerializeProvider<T?>
        where T : struct
        where TProvider : ISerializeProvider<T>
    {
        public static Serialize<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;
        private Serialize() : base(TProvider.SerializeInstance) { }
    }

    public class SerializeInstance<T, TProxy>(TProxy proxy) : ISerialize<T?>
        where T : struct
        where TProxy : ISerialize<T>
    {
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

    public sealed class Deserialize<T, TProvider>
        : DeserializeInstance<T, IDeserialize<T>>,
          IDeserializeProvider<T?>
        where T : struct
        where TProvider : IDeserializeProvider<T>
    {
        public static Deserialize<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;
        private Deserialize() : base(TProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<T, TProxy>(TProxy proxy) : IDeserialize<T?>
        where T : struct
        where TProxy : IDeserialize<T>
    {
        private readonly BoxProxy _boxProxy = new(proxy);

        public T? Deserialize(IDeserializer deserializer)
        {
            return (T?)deserializer.ReadNullableRef<object, BoxProxy>(_boxProxy);
        }

        private sealed class BoxProxy(TProxy underlyingProxy) : IDeserialize<object>
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
    public sealed class Serialize<T, TProvider> : SerializeInstance<T, ISerialize<T>>, ISerializeProvider<T?>
        where T : class
        where TProvider : ISerializeProvider<T>
    {
        public static Serialize<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;

        private Serialize() : base(TProvider.SerializeInstance) { }
    }

    public class SerializeInstance<T, TProxy>(TProxy proxy) : ISerialize<T?>
        where T : class
        where TProxy : ISerialize<T>
    {

        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is null)
            {
                serializer.WriteNull();
            }
            else
            {
                proxy.Serialize(value, serializer);
            }
        }
    }

    public sealed class Deserialize<T, TProvider> : DeserializeInstance<T, IDeserialize<T>>, IDeserializeProvider<T?>
        where T : class
        where TProvider : IDeserializeProvider<T>
    {
        public static Deserialize<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => NullableInfoCache<TProvider>.Instance;

        private Deserialize() : base(TProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<T, TProxy>(TProxy proxy) : IDeserialize<T?>
        where T : class
        where TProxy : IDeserialize<T>
    {
        public T? Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef<T, TProxy>(proxy);
        }
    }
}