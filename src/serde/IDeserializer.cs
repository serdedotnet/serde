using System;
using System.Buffers;
using System.Threading.Tasks;

namespace Serde;

public interface IDeserializer : IDisposable
{
    Task<T?> ReadNullableRef<T>(IDeserialize<T> deserialize) where T : class;

    Task<bool> ReadBool();
    Task<char> ReadChar();
    Task<byte> ReadU8();
    Task<ushort> ReadU16();
    Task<uint> ReadU32();
    Task<ulong> ReadU64();
    Task<sbyte> ReadI8();
    Task<short> ReadI16();
    Task<int> ReadI32();
    Task<long> ReadI64();
    Task<float> ReadF32();
    Task<double> ReadF64();
    Task<decimal> ReadDecimal();
    Task<string> ReadString();
    Task<DateTime> ReadDateTime();
    Task ReadBytes(IBufferWriter<byte> writer);
    ITypeDeserializer ReadType(ISerdeInfo typeInfo);
}

public static class IDeserializerExt
{
    public static Task<T> ReadValue<T, TProvider>(this IDeserializer deserializer)
        where TProvider : IDeserializeProvider<T>
    {
        var de = DeserializeProvider.GetDeserialize<T, TProvider>();
        return de.Deserialize(deserializer);
    }
    public static Task<T> ReadValue<T>(this IDeserializer deserializer)
        where T : IDeserializeProvider<T>
        => deserializer.ReadValue<T, T>();
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
    /// should return the index. If the end of the type is reached, the method should return <see
    /// cref="EndOfType" />. If the field is not found, the method should return <see
    /// cref="IndexNotFound" />. To retrieve the name of the missing field, use <see
    /// cref="TryReadIndexWithName" />.
    /// </summary>
    Task<int> TryReadIndex(ISerdeInfo info);

    /// <summary>
    /// Try to read the index of the next field in the type. If the index is found, the method
    /// should return the index and set errorName to null. If the end of the type is reached, the
    /// method should return <see cref="EndOfType" /> and set errorName to null. If the field is not
    /// found, the method should return <see cref="IndexNotFound" /> and set errorName to the name
    /// of the missing field, or the best-possible user-facing name.
    /// </summary>
    Task<(int, string? errorName)> TryReadIndexWithName(ISerdeInfo info);

    Task<T> ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize)
        where T : class?;

    Task SkipValue(ISerdeInfo info, int index);
    Task<bool> ReadBool(ISerdeInfo info, int index);
    Task<char> ReadChar(ISerdeInfo info, int index);
    Task<byte> ReadU8(ISerdeInfo info, int index);
    Task<ushort> ReadU16(ISerdeInfo info, int index);
    Task<uint> ReadU32(ISerdeInfo info, int index);
    Task<ulong> ReadU64(ISerdeInfo info, int index);
    Task<sbyte> ReadI8(ISerdeInfo info, int index);
    Task<short> ReadI16(ISerdeInfo info, int index);
    Task<int> ReadI32(ISerdeInfo info, int index);
    Task<long> ReadI64(ISerdeInfo info, int index);
    Task<float> ReadF32(ISerdeInfo info, int index);
    Task<double> ReadF64(ISerdeInfo info, int index);
    Task<decimal> ReadDecimal(ISerdeInfo info, int index);
    Task<string> ReadString(ISerdeInfo info, int index);
    Task<DateTime> ReadDateTime(ISerdeInfo info, int index);
    Task ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer);
}

public static class ITypeDeserializerExt
{
    public static async Task<T> ReadValue<T, TProvider>(this ITypeDeserializer deserializeType, ISerdeInfo info, int index)
        where T : class?
        where TProvider : IDeserializeProvider<T>
    {
        return await deserializeType.ReadValue(info, index, TProvider.Instance);
    }

    public static async Task<T> ReadBoxedValue<T, TProvider>(this ITypeDeserializer deserializeType, ISerdeInfo info, int index)
        where TProvider : IDeserializeProvider<T>
    {
        return (T)(await deserializeType.ReadValue(info, index, BoxProxy.De<T, TProvider>.Instance))!;
    }
}
