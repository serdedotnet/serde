using System;
using System.Buffers;
using System.Threading.Tasks;

namespace Serde;

public interface IDeserializer : IDisposable
{
    ValueTask<T?> ReadNullableRef<T>(IDeserialize<T> deserialize) where T : class;

    ValueTask<bool> ReadBool();
    ValueTask<char> ReadChar();
    ValueTask<byte> ReadU8();
    ValueTask<ushort> ReadU16();
    ValueTask<uint> ReadU32();
    ValueTask<ulong> ReadU64();
    ValueTask<sbyte> ReadI8();
    ValueTask<short> ReadI16();
    ValueTask<int> ReadI32();
    ValueTask<long> ReadI64();
    ValueTask<float> ReadF32();
    ValueTask<double> ReadF64();
    ValueTask<decimal> ReadDecimal();
    ValueTask<string> ReadString();
    ValueTask<DateTime> ReadDateTime();
    ValueTask ReadBytes(IBufferWriter<byte> writer);
    ITypeDeserializer ReadType(ISerdeInfo typeInfo);
}

public static class IDeserializerExt
{
    public static ValueTask<T> ReadValue<T, TProvider>(this IDeserializer deserializer)
        where TProvider : IDeserializeProvider<T>
    {
        var de = DeserializeProvider.GetDeserialize<T, TProvider>();
        return de.Deserialize(deserializer);
    }
    public static ValueTask<T> ReadValue<T>(this IDeserializer deserializer)
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
    /// should return the index and set <paramref name="errorName" /> to null. If the end of the
    /// type is reached, the method should return <see cref="EndOfType" /> and set <paramref
    /// name="errorName" /> to null. If the field is not found, the method should return <see
    /// cref="IndexNotFound" /> and set <paramref name="errorName" /> to the name of the missing
    /// field, or the best-possible user-facing name.
    /// </summary>
    ValueTask<int> TryReadIndex(ISerdeInfo info, out string? errorName);

    ValueTask<T> ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize)
        where T : class?;

    ValueTask SkipValue(ISerdeInfo info, int index);
    ValueTask<bool> ReadBool(ISerdeInfo info, int index);
    ValueTask<char> ReadChar(ISerdeInfo info, int index);
    ValueTask<byte> ReadU8(ISerdeInfo info, int index);
    ValueTask<ushort> ReadU16(ISerdeInfo info, int index);
    ValueTask<uint> ReadU32(ISerdeInfo info, int index);
    ValueTask<ulong> ReadU64(ISerdeInfo info, int index);
    ValueTask<sbyte> ReadI8(ISerdeInfo info, int index);
    ValueTask<short> ReadI16(ISerdeInfo info, int index);
    ValueTask<int> ReadI32(ISerdeInfo info, int index);
    ValueTask<long> ReadI64(ISerdeInfo info, int index);
    ValueTask<float> ReadF32(ISerdeInfo info, int index);
    ValueTask<double> ReadF64(ISerdeInfo info, int index);
    ValueTask<decimal> ReadDecimal(ISerdeInfo info, int index);
    ValueTask<string> ReadString(ISerdeInfo info, int index);
    ValueTask<DateTime> ReadDateTime(ISerdeInfo info, int index);
    ValueTask ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer);
}

public static class ITypeDeserializerExt
{
    public static async ValueTask<T> ReadValue<T, TProvider>(this ITypeDeserializer deserializeType, ISerdeInfo info, int index)
        where T : class?
        where TProvider : IDeserializeProvider<T>
    {
        return await deserializeType.ReadValue(info, index, TProvider.Instance);
    }

    public static async ValueTask<T> ReadBoxedValue<T, TProvider>(this ITypeDeserializer deserializeType, ISerdeInfo info, int index)
        where TProvider : IDeserializeProvider<T>
    {
        return (T)(await deserializeType.ReadValue(info, index, BoxProxy.De<T, TProvider>.Instance))!;
    }
}
