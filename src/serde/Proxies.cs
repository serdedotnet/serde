// Contains implementations of data interfaces for core types

using System;
using System.Threading.Tasks;

namespace Serde;

internal interface ISerdePrimitive<TSelf, T> : ISerde<T>, ISerdeProvider<TSelf, TSelf, T>
    where TSelf : ISerdePrimitive<TSelf, T>
{
    /// <summary>
    /// Abstract static to force all primitives to provide a convenient static accessor.
    /// </summary>
    public static new abstract ISerdeInfo SerdeInfo { get; }
}

public sealed class BoolProxy : ISerdePrimitive<BoolProxy, bool>
{
    public static BoolProxy Instance { get; } = new();

    private BoolProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.Bool);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "bool";

#if NET11_0_OR_GREATER
    async Task ISerialize<bool>.Serialize(bool value, ISerializer serializer) => await serializer.WriteBool(value);
#else
    void ISerialize<bool>.Serialize(bool value, ISerializer serializer) => serializer.WriteBool(value);
#endif

    bool IDeserialize<bool>.Deserialize(IDeserializer deserializer) => deserializer.ReadBool();

#if NET11_0_OR_GREATER
    async Task ISerialize<bool>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, bool value
    )
    {
        await serializer.WriteBool(info, index, value);
    }
#else
    void ISerialize<bool>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        bool value
    ) => serializer.WriteBool(info, index, value);
#endif

    bool IDeserialize<bool>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadBool(info, index);
}

public sealed class CharProxy : ISerdePrimitive<CharProxy, char>
{
    public static CharProxy Instance { get; } = new();

    private CharProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.Char);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "char";

#if NET11_0_OR_GREATER
    async Task ISerialize<char>.Serialize(char value, ISerializer serializer) => await serializer.WriteChar(value);
#else
    void ISerialize<char>.Serialize(char value, ISerializer serializer) => serializer.WriteChar(value);
#endif

    char IDeserialize<char>.Deserialize(IDeserializer deserializer) => deserializer.ReadChar();

#if NET11_0_OR_GREATER
    async Task ISerialize<char>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, char value
    )
    {
        await serializer.WriteChar(info, index, value);
    }
#else
    void ISerialize<char>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        char value
    ) => serializer.WriteChar(info, index, value);
#endif

    char IDeserialize<char>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadChar(info, index);
}

public sealed class U8Proxy : ISerdePrimitive<U8Proxy, byte>
{
    public static U8Proxy Instance { get; } = new();

    private U8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U8);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "byte";

#if NET11_0_OR_GREATER
    async Task ISerialize<byte>.Serialize(byte value, ISerializer serializer) => await serializer.WriteU8(value);
#else
    void ISerialize<byte>.Serialize(byte value, ISerializer serializer) => serializer.WriteU8(value);
#endif

    byte IDeserialize<byte>.Deserialize(IDeserializer deserializer) => deserializer.ReadU8();

#if NET11_0_OR_GREATER
    async Task ISerialize<byte>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, byte value
    )
    {
        await serializer.WriteU8(info, index, value);
    }
#else
    void ISerialize<byte>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        byte value
    ) => serializer.WriteU8(info, index, value);
#endif

    byte IDeserialize<byte>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadU8(info, index);
}

public sealed class U16Proxy : ISerdePrimitive<U16Proxy, ushort>
{
    public static U16Proxy Instance { get; } = new();

    private U16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U16);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "ushort";

#if NET11_0_OR_GREATER
    async Task ISerialize<ushort>.Serialize(ushort value, ISerializer serializer) => await serializer.WriteU16(value);
#else
    void ISerialize<ushort>.Serialize(ushort value, ISerializer serializer) => serializer.WriteU16(value);
#endif

    ushort IDeserialize<ushort>.Deserialize(IDeserializer deserializer) => deserializer.ReadU16();

    ushort IDeserialize<ushort>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadU16(info, index);

#if NET11_0_OR_GREATER
    async Task ISerialize<ushort>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, ushort value
    )
    {
        await serializer.WriteU16(info, index, value);
    }
#else
    void ISerialize<ushort>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        ushort value
    ) => serializer.WriteU16(info, index, value);
#endif
}

public sealed class U32Proxy : ISerdePrimitive<U32Proxy, uint>
{
    public static U32Proxy Instance { get; } = new();

    private U32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U32);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "uint";

#if NET11_0_OR_GREATER
    async Task ISerialize<uint>.Serialize(uint value, ISerializer serializer) => await serializer.WriteU32(value);
#else
    void ISerialize<uint>.Serialize(uint value, ISerializer serializer) => serializer.WriteU32(value);
#endif

    uint IDeserialize<uint>.Deserialize(IDeserializer deserializer) => deserializer.ReadU32();

    uint IDeserialize<uint>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadU32(info, index);

#if NET11_0_OR_GREATER
    async Task ISerialize<uint>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, uint value
    )
    {
        await serializer.WriteU32(info, index, value);
    }
#else
    void ISerialize<uint>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        uint value
    ) => serializer.WriteU32(info, index, value);
#endif
}

public sealed class U64Proxy : ISerdePrimitive<U64Proxy, ulong>
{
    public static U64Proxy Instance { get; } = new();

    private U64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U64);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "ulong";

#if NET11_0_OR_GREATER
    async Task ISerialize<ulong>.Serialize(ulong value, ISerializer serializer) => await serializer.WriteU64(value);
#else
    void ISerialize<ulong>.Serialize(ulong value, ISerializer serializer) => serializer.WriteU64(value);
#endif

    ulong IDeserialize<ulong>.Deserialize(IDeserializer deserializer) => deserializer.ReadU64();

#if NET11_0_OR_GREATER
    async Task ISerialize<ulong>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, ulong value
    )
    {
        await serializer.WriteU64(info, index, value);
    }
#else
    void ISerialize<ulong>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        ulong value
    ) => serializer.WriteU64(info, index, value);
#endif

    ulong IDeserialize<ulong>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadU64(info, index);
}

public sealed class U128Proxy : ISerdePrimitive<U128Proxy, UInt128>
{
    public static U128Proxy Instance { get; } = new();

    private U128Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U128);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "System.UInt128";

#if NET11_0_OR_GREATER
    async Task ISerialize<UInt128>.Serialize(UInt128 value, ISerializer serializer) => await serializer.WriteU128(value);
#else
    void ISerialize<UInt128>.Serialize(UInt128 value, ISerializer serializer) => serializer.WriteU128(value);
#endif

    UInt128 IDeserialize<UInt128>.Deserialize(IDeserializer deserializer) =>
        deserializer.ReadU128();

#if NET11_0_OR_GREATER
    async Task ISerialize<UInt128>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, UInt128 value
    )
    {
        await serializer.WriteU128(info, index, value);
    }
#else
    void ISerialize<UInt128>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        UInt128 value
    ) => serializer.WriteU128(info, index, value);
#endif

    UInt128 IDeserialize<UInt128>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadU128(info, index);
}

public sealed class I8Proxy : ISerdePrimitive<I8Proxy, sbyte>
{
    public static I8Proxy Instance { get; } = new();

    private I8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I8);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "sbyte";

#if NET11_0_OR_GREATER
    async Task ISerialize<sbyte>.Serialize(sbyte value, ISerializer serializer) => await serializer.WriteI8(value);
#else
    void ISerialize<sbyte>.Serialize(sbyte value, ISerializer serializer) => serializer.WriteI8(value);
#endif

    sbyte IDeserialize<sbyte>.Deserialize(IDeserializer deserializer) => deserializer.ReadI8();

#if NET11_0_OR_GREATER
    async Task ISerialize<sbyte>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, sbyte value
    )
    {
        await serializer.WriteI8(info, index, value);
    }
#else
    void ISerialize<sbyte>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        sbyte value
    ) => serializer.WriteI8(info, index, value);
#endif

    sbyte IDeserialize<sbyte>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadI8(info, index);
}

public sealed class I16Proxy : ISerdePrimitive<I16Proxy, short>
{
    public static I16Proxy Instance { get; } = new();

    private I16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I16);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "short";

#if NET11_0_OR_GREATER
    async Task ISerialize<short>.Serialize(short value, ISerializer serializer) => await serializer.WriteI16(value);
#else
    void ISerialize<short>.Serialize(short value, ISerializer serializer) => serializer.WriteI16(value);
#endif

    short IDeserialize<short>.Deserialize(IDeserializer deserializer) => deserializer.ReadI16();

#if NET11_0_OR_GREATER
    async Task ISerialize<short>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, short value
    )
    {
        await serializer.WriteI16(info, index, value);
    }
#else
    void ISerialize<short>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        short value
    ) => serializer.WriteI16(info, index, value);
#endif

    short IDeserialize<short>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadI16(info, index);
}

public sealed class I32Proxy : ISerdePrimitive<I32Proxy, int>
{
    public static I32Proxy Instance { get; } = new();

    private I32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I32);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "int";

#if NET11_0_OR_GREATER
    async Task ISerialize<int>.Serialize(int value, ISerializer serializer) => await serializer.WriteI32(value);
#else
    void ISerialize<int>.Serialize(int value, ISerializer serializer) => serializer.WriteI32(value);
#endif

    int IDeserialize<int>.Deserialize(IDeserializer deserializer) => deserializer.ReadI32();

#if NET11_0_OR_GREATER
    async Task ISerialize<int>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, int value
    )
    {
        await serializer.WriteI32(info, index, value);
    }
#else
    void ISerialize<int>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        int value
    ) => serializer.WriteI32(info, index, value);
#endif

    int IDeserialize<int>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadI32(info, index);
}

public sealed class I64Proxy : ISerdePrimitive<I64Proxy, long>
{
    public static I64Proxy Instance { get; } = new();

    private I64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I64);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "long";

#if NET11_0_OR_GREATER
    async Task ISerialize<long>.Serialize(long value, ISerializer serializer) => await serializer.WriteI64(value);
#else
    void ISerialize<long>.Serialize(long value, ISerializer serializer) => serializer.WriteI64(value);
#endif

    long IDeserialize<long>.Deserialize(IDeserializer deserializer) => deserializer.ReadI64();

#if NET11_0_OR_GREATER
    async Task ISerialize<long>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, long value
    )
    {
        await serializer.WriteI64(info, index, value);
    }
#else
    void ISerialize<long>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        long value
    ) => serializer.WriteI64(info, index, value);
#endif

    long IDeserialize<long>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadI64(info, index);
}

public sealed class I128Proxy : ISerdePrimitive<I128Proxy, Int128>
{
    public static I128Proxy Instance { get; } = new();

    private I128Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I128);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "System.Int128";

#if NET11_0_OR_GREATER
    async Task ISerialize<Int128>.Serialize(Int128 value, ISerializer serializer) => await serializer.WriteI128(value);
#else
    void ISerialize<Int128>.Serialize(Int128 value, ISerializer serializer) => serializer.WriteI128(value);
#endif

    Int128 IDeserialize<Int128>.Deserialize(IDeserializer deserializer) => deserializer.ReadI128();

#if NET11_0_OR_GREATER
    async Task ISerialize<Int128>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, Int128 value
    )
    {
        await serializer.WriteI128(info, index, value);
    }
#else
    void ISerialize<Int128>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        Int128 value
    ) => serializer.WriteI128(info, index, value);
#endif

    Int128 IDeserialize<Int128>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadI128(info, index);
}

public sealed class F16Proxy : ISerdePrimitive<F16Proxy, Half>
{
    public static F16Proxy Instance { get; } = new();

    private F16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("half", PrimitiveKind.F16);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<Half>.Serialize(Half value, ISerializer serializer) => await serializer.WriteF16(value);
#else
    void ISerialize<Half>.Serialize(Half value, ISerializer serializer) => serializer.WriteF16(value);
#endif

    public Half Deserialize(IDeserializer deserializer) => deserializer.ReadF16();

#if NET11_0_OR_GREATER
    async Task ISerialize<Half>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, Half value
    )
    {
        await serializer.WriteF16(info, index, value);
    }
#else
    void ISerialize<Half>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        Half value
    ) => serializer.WriteF16(info, index, value);
#endif

    Half IDeserialize<Half>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadF16(info, index);
}

public sealed class F32Proxy : ISerdePrimitive<F32Proxy, float>
{
    public static F32Proxy Instance { get; } = new();

    private F32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("float", PrimitiveKind.F32);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<float>.Serialize(float value, ISerializer serializer) => await serializer.WriteF32(value);
#else
    void ISerialize<float>.Serialize(float value, ISerializer serializer) => serializer.WriteF32(value);
#endif

    public float Deserialize(IDeserializer deserializer) => deserializer.ReadF32();

#if NET11_0_OR_GREATER
    async Task ISerialize<float>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, float value
    )
    {
        await serializer.WriteF32(info, index, value);
    }
#else
    void ISerialize<float>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        float value
    ) => serializer.WriteF32(info, index, value);
#endif

    float IDeserialize<float>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadF32(info, index);
}

public sealed class F64Proxy : ISerdePrimitive<F64Proxy, double>
{
    public static F64Proxy Instance { get; } = new();

    private F64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("double", PrimitiveKind.F64);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<double>.Serialize(double value, ISerializer serializer) => await serializer.WriteF64(value);
#else
    void ISerialize<double>.Serialize(double value, ISerializer serializer) => serializer.WriteF64(value);
#endif

    double IDeserialize<double>.Deserialize(IDeserializer deserializer) => deserializer.ReadF64();

#if NET11_0_OR_GREATER
    async Task ISerialize<double>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, double value
    )
    {
        await serializer.WriteF64(info, index, value);
    }
#else
    void ISerialize<double>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        double value
    ) => serializer.WriteF64(info, index, value);
#endif

    double IDeserialize<double>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadF64(info, index);
}

public sealed class DecimalProxy : ISerdePrimitive<DecimalProxy, decimal>
{
    public static DecimalProxy Instance { get; } = new();

    private DecimalProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("decimal", PrimitiveKind.Decimal);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<decimal>.Serialize(decimal value, ISerializer serializer) => await serializer.WriteDecimal(value);
#else
    void ISerialize<decimal>.Serialize(decimal value, ISerializer serializer) => serializer.WriteDecimal(value);
#endif

    decimal IDeserialize<decimal>.Deserialize(IDeserializer deserializer) =>
        deserializer.ReadDecimal();

    decimal IDeserialize<decimal>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadDecimal(info, index);

#if NET11_0_OR_GREATER
    async Task ISerialize<decimal>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, decimal value
    )
    {
        await serializer.WriteDecimal(info, index, value);
    }
#else
    void ISerialize<decimal>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        decimal value
    ) => serializer.WriteDecimal(info, index, value);
#endif
}

public sealed class StringProxy : ISerdePrimitive<StringProxy, string>
{
    public static StringProxy Instance { get; } = new();

    private StringProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "string";

#if NET11_0_OR_GREATER
    async Task ISerialize<string>.Serialize(string value, ISerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value);
        await serializer.WriteString(value);
    }
#else
    void ISerialize<string>.Serialize(string value, ISerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value);
        serializer.WriteString(value);
    }
#endif

    public string Deserialize(IDeserializer deserializer) => deserializer.ReadString();

    string IDeserialize<string>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadString(info, index);

#if NET11_0_OR_GREATER
    async Task ISerialize<string>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, string value
    )
    {
        await serializer.WriteString(info, index, value);
    }
#else
    void ISerialize<string>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        string value
    ) => serializer.WriteString(info, index, value);
#endif
}

#if !NET11_0_OR_GREATER
public static class BoxProxy
{
    [Obsolete("BoxProxy is deprecated. Use ReadValue and WriteValue instead.")]
    public sealed class Ser<T> : ISerialize<object?>
    {
        private readonly ISerialize<T> _underlying;

        public Ser(ISerialize<T> underlying)
        {
            _underlying = underlying;
        }

        public ISerdeInfo SerdeInfo => _underlying.SerdeInfo;

        public void Serialize(object? value, ISerializer serializer)
        {
            _underlying.Serialize((T)value!, serializer);
        }
    }

    [Obsolete("BoxProxy is deprecated. Use ReadValue and WriteValue instead.")]
    public sealed class Ser<T, TProvider>
        where TProvider : ISerializeProvider<T>
    {
        public static readonly Ser<T> Instance = new Ser<T>(TProvider.Instance);
    }

    [Obsolete("BoxProxy is deprecated. Use ReadValue and WriteValue instead.")]
    public sealed class De<T>(IDeserialize<T> _underlying) : ITypeDeserialize<T>
    {
        public ISerdeInfo SerdeInfo => _underlying.SerdeInfo;

        public T Deserialize(IDeserializer deserializer) => _underlying.Deserialize(deserializer);

        public T Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index) =>
            (T)deserializer.ReadValue(info, index, this)!;
    }

    [Obsolete("BoxProxy is deprecated. Use ReadValue and WriteValue instead.")]
    public static class De<T, TProvider>
        where TProvider : IDeserializeProvider<T>
    {
        public static readonly De<T> Instance = new De<T>(TProvider.Instance);
    }
}
#endif

public static class NullableProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<T?>, ISerializeProvider<T?>
        where T : struct
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } =
            Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly ISerialize<T> proxy = TProvider.Instance;

        private Ser() { }

#if NET11_0_OR_GREATER
        async Task ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is { } notnull) await proxy.Serialize(notnull, serializer);
            else await serializer.WriteNull();
        }
#else
        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is { } notnull)
            {
                proxy.Serialize(notnull, serializer);
            }
            else
            {
                serializer.WriteNull();
            }
        }
#endif

#if NET11_0_OR_GREATER
        async Task ISerialize<T?>.SerializeAsField(
            ITypeSerializer typeSerializer, ISerdeInfo serdeInfo, int index, T? value
        )
        {
            if (value is { } notnull)
            {
                await proxy.SerializeAsField(typeSerializer, serdeInfo, index, notnull);
            }
            else
            {
                await typeSerializer.WriteNull(serdeInfo, index);
            }
        }
#else
        void ISerialize<T?>.SerializeAsField(
            ITypeSerializer typeSerializer,
            ISerdeInfo serdeInfo,
            int index,
            T? value
        )
        {
            if (value is { } notnull)
            {
                proxy.SerializeAsField(typeSerializer, serdeInfo, index, notnull);
            }
            else
            {
                typeSerializer.WriteNull(serdeInfo, index);
            }
        }
#endif
    }

    public sealed class De<T, TProvider> : IDeserialize<T?>, IDeserializeProvider<T?>
        where T : struct
        where TProvider : IDeserializeProvider<T>
    {
        public static De<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } =
            Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);
        private readonly IDeserialize<T> proxy = TProvider.Instance;

        private De() { }

        public T? Deserialize(IDeserializer deserializer)
        {
            if (deserializer.TryReadNull())
            {
                return null;
            }
            else
            {
                return proxy.Deserialize(deserializer);
            }
        }

        T? IDeserialize<T?>.DeserializeAsField(
            ITypeDeserializer typeDeserializer,
            ISerdeInfo serdeInfo,
            int index
        )
        {
            var d = typeDeserializer.ReadFieldStart(serdeInfo, index);
            T? result = d.TryReadNull() ? null : proxy.Deserialize(d);
            typeDeserializer.ReadFieldEnd(serdeInfo, index, d);
            return result;
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
        public ISerdeInfo SerdeInfo { get; } =
            Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly ISerialize<T> _ser = TProvider.Instance;

        private Ser() { }

#if NET11_0_OR_GREATER
        async Task ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is null) await serializer.WriteNull();
            else await _ser.Serialize(value, serializer);
        }
#else
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
#endif
    }

    public sealed class De<T, TProvider> : IDeserialize<T?>, IDeserializeProvider<T?>
        where T : class
        where TProvider : IDeserializeProvider<T>
    {
        public static De<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } =
            Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly IDeserialize<T> _de = TProvider.Instance;

        private De() { }

        public T? Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef(_de);
        }
    }
}

public sealed class GuidProxy : ISerdePrimitive<GuidProxy, Guid>
{
    public static GuidProxy Instance { get; } = new();

    private GuidProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("System.Guid", PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<Guid>.Serialize(Guid value, ISerializer serializer) => await serializer.WriteString(value.ToString());
#else
    void ISerialize<Guid>.Serialize(Guid value, ISerializer serializer)
    {
        var bytes = value.ToString();
        serializer.WriteString(bytes);
    }
#endif

    public Guid Deserialize(IDeserializer deserializer)
    {
        var bytes = deserializer.ReadString();
        return Guid.Parse(bytes);
    }

#if NET11_0_OR_GREATER
    async Task ISerialize<Guid>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, Guid value
    )
    {
        await serializer.WriteString(info, index, value.ToString());
    }
#else
    void ISerialize<Guid>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        Guid value
    )
    {
        var bytes = value.ToString();
        serializer.WriteString(info, index, bytes);
    }
#endif

    Guid IDeserialize<Guid>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    )
    {
        var bytes = deserializer.ReadString(info, index);
        return Guid.Parse(bytes);
    }
}
