
using System;

namespace Serde;

/// <summary>
/// The driving interface for deserializing a given type. This interface separates deserialization
/// from the type being deserialized. This allows for deserialization to be implemented by a
/// different type than the one being deserialized. All underlying deserialization primitives should
/// be based on this interface. However, types which implement their own deserialization logic
/// should also implement <see cref="IDeserializeProvider{T}"/>.
/// </summary>
public interface IDeserialize<T> : ISerdeInfoProvider
{
    T Deserialize(IDeserializer deserializer);
}

public interface IDeserializeProvider<T>
{
    abstract static IDeserialize<T> Instance { get; }
}

public static class DeserializeProvider
{
    public static IDeserialize<T> GetDeserialize<T, TProvider>()
        where TProvider : IDeserializeProvider<T>
        => TProvider.Instance;
}

public static partial class DeserializeExtensions
{
    public static IDeserialize<T> GetDeserialize<T>(this T? _)
        where T : IDeserializeProvider<T>
        => T.Instance;
}

public interface IDeserializer : IDisposable
{
    T? ReadNullableRef<T>(IDeserialize<T> deserialize) where T : class;

    bool ReadBool();
    char ReadChar();
    byte ReadU8();
    ushort ReadU16();
    uint ReadU32();
    ulong ReadU64();
    sbyte ReadI8();
    short ReadI16();
    int ReadI32();
    long ReadI64();
    float ReadF32();
    double ReadF64();
    decimal ReadDecimal();
    string ReadString();
    ITypeDeserializer ReadType(ISerdeInfo typeInfo);
}

public interface ITypeDeserializer
{
    public const int EndOfType = -1;
    public const int IndexNotFound = -2;

    /// <summary>
    /// Returns the number of elements in the collection, or null if the size is not known.
    /// </summary>
    public int? SizeOpt { get; }

    /// <summary>
    /// Try to read the index of the next field in the type. If the index is found, the method
    /// should return the index and set <paramref name="errorName" /> to null. If the end of the
    /// type is reached, the method should return <see cref="EndOfType" /> and set <paramref
    /// name="errorName" /> to null. If the field is not found, the method should return <see
    /// cref="IndexNotFound" /> and set <paramref name="errorName" /> to the name of the missing
    /// field, or the best-possible user-facing name.
    /// </summary>
    int TryReadIndex(ISerdeInfo info, out string? errorName);

    T ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize)
        where T : class?;

    void SkipValue(ISerdeInfo info, int index);
    bool ReadBool(ISerdeInfo info, int index);
    char ReadChar(ISerdeInfo info, int index);
    byte ReadU8(ISerdeInfo info, int index);
    ushort ReadU16(ISerdeInfo info, int index);
    uint ReadU32(ISerdeInfo info, int index);
    ulong ReadU64(ISerdeInfo info, int index);
    sbyte ReadI8(ISerdeInfo info, int index);
    short ReadI16(ISerdeInfo info, int index);
    int ReadI32(ISerdeInfo info, int index);
    long ReadI64(ISerdeInfo info, int index);
    float ReadF32(ISerdeInfo info, int index);
    double ReadF64(ISerdeInfo info, int index);
    decimal ReadDecimal(ISerdeInfo info, int index);
    string ReadString(ISerdeInfo info, int index);
}

public static class ITypeDeserializerExt
{
    public static T ReadValue<T, TProvider>(this ITypeDeserializer deserializeType, ISerdeInfo info, int index)
        where T : class?
        where TProvider : IDeserializeProvider<T>
    {
        return deserializeType.ReadValue(info, index, TProvider.Instance);
    }

    private sealed class BoxProxy<T, TProvider> : IDeserialize<object?>
        where TProvider : IDeserializeProvider<T>
    {
        public static readonly BoxProxy<T, TProvider> Instance = new BoxProxy<T, TProvider>();

        private readonly IDeserialize<T> _deserialize = TProvider.Instance;
        public ISerdeInfo SerdeInfo => _deserialize.SerdeInfo;

        public BoxProxy() { }


        public object? Deserialize(IDeserializer deserializer)
        {
            return _deserialize.Deserialize(deserializer);
        }
    }

    public static T ReadBoxedValue<T, TProvider>(this ITypeDeserializer deserializeType, ISerdeInfo info, int index)
        where TProvider : IDeserializeProvider<T>
    {
        return (T)deserializeType.ReadValue(info, index, BoxProxy<T, TProvider>.Instance)!;
    }
}