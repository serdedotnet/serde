
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Serde;

public interface IDeserializeProvider<T> : ISerdeInfoProvider
{
    abstract static IDeserialize<T> DeserializeInstance { get; }
}

/// <summary>
/// The driving interface for deserializing a given type. This interface separates deserialization
/// from the type being deserialized. This allows for deserialization to be implemented by a
/// different type than the one being deserialized. All underlying deserialization primitives should
/// be based on this interface. However, types which implement their own deserialization logic
/// should also implement <see cref="IDeserializeProvider{T}"/>.
/// </summary>
public interface IDeserialize<T>
{
    T Deserialize(IDeserializer deserializer);
}

public static partial class DeserializeExtensions
{
    public static IDeserialize<T> GetDeserialize<T>(this T? _)
        where T : IDeserializeProvider<T>
        => T.DeserializeInstance;
}

/// <summary>
/// Thrown from implementations of <see cref="IDeserializer" />. Indicates that an unexpected
/// value was seen in the input which cannot be converted to the target type.
/// </summary>
public class DeserializeException(string msg) : Exception(msg)
{
    public static DeserializeException UnassignedMember() => throw new DeserializeException("Not all members were assigned.");

    public static DeserializeException UnknownMember(string name, ISerdeInfo info)
        => new DeserializeException($"Could not find member named '{name ?? "<null>"}' in type '{info.Name}'.");

    public static DeserializeException WrongItemCount(int expected, int actual)
        => new DeserializeException($"Expected {expected} items, got {actual}.");
}

public interface IDeserializeVisitor<T>
{
    string ExpectedTypeName { get; }
    T VisitBool(bool b) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitChar(char c) => VisitString(c.ToString());
    T VisitByte(byte b) => VisitU64(b);
    T VisitU16(ushort u16) => VisitU64(u16);
    T VisitU32(uint u32) => VisitU64(u32);
    T VisitU64(ulong u64) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitSByte(sbyte b) => VisitI64(b);
    T VisitI16(short i16) => VisitI64(i16);
    T VisitI32(int i32) => VisitI64(i32);
    T VisitI64(long i64) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitFloat(float f) => VisitDouble(f);
    T VisitDouble(double d) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitDecimal(decimal d) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitString(string s) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitUtf8Span(ReadOnlySpan<byte> s) => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitEnumerable<D>(ref D d) where D : IDeserializeEnumerable
        => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitDictionary<D>(ref D d) where D : IDeserializeDictionary
        => throw new DeserializeException("Expected type " + ExpectedTypeName);
    T VisitNull() => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
    T VisitNotNull(IDeserializer d) => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
}

public interface IDeserializeEnumerable
{
    bool TryGetNext<T, TProxy>(TProxy deserialize, [MaybeNullWhen(false)] out T next)
        where TProxy : IDeserialize<T>;
    int? SizeOpt { get; }
}

public interface IDeserializeDictionary
{
    bool TryGetNextKey<K, D>(D deserialize, [MaybeNullWhen(false)] out K next)
        where D : IDeserialize<K>;
    V GetNextValue<V, D>(D deserialize) where D : IDeserialize<V>;
    bool TryGetNextEntry<K, V, DK, DV>(DK dk, DV dv, [MaybeNullWhen(false)] out (K, V) next)
        where DK : IDeserialize<K>
        where DV : IDeserialize<V>;
    int? SizeOpt { get; }
}

public interface IDeserializeCollection
{
    int? SizeOpt { get; }

    bool TryReadValue<T, D>(ISerdeInfo typeInfo, D deserialize, [MaybeNullWhen(false)] out T next)
        where D : IDeserialize<T>;
}

public interface IDeserializeType
{
    public const int EndOfType = -1;
    public const int IndexNotFound = -2;

    /// <summary>
    /// Try to read the index of the next field in the type. If the index is found, the method
    /// should return the index and set <paramref name="errorName" /> to null. If the end of the
    /// type is reached, the method should return <see cref="EndOfType" /> and set <paramref
    /// name="errorName" /> to null. If the field is not found, the method should return <see
    /// cref="IndexNotFound" /> and set <paramref name="errorName" /> to the name of the missing
    /// field, or the best-possible user-facing name.
    /// </summary>
    int TryReadIndex(ISerdeInfo map, out string? errorName);

    T ReadValue<T, D>(int index, D deserialize) where D : IDeserialize<T>;

    void SkipValue();

    bool ReadBool(int index);
    char ReadChar(int index);
    byte ReadByte(int index);
    ushort ReadU16(int index);
    uint ReadU32(int index);
    ulong ReadU64(int index);
    sbyte ReadSByte(int index);
    short ReadI16(int index);
    int ReadI32(int index);
    long ReadI64(int index);
    float ReadFloat(int index);
    double ReadDouble(int index);
    decimal ReadDecimal(int index);
    string ReadString(int index);
}

public static class IDeserializeTypeExt
{
    public static T ReadValue<T, TProvider>(this IDeserializeType deserializeType, int index)
        where TProvider : IDeserializeProvider<T>
    {
        return deserializeType.ReadValue<T, IDeserialize<T>>(index, TProvider.DeserializeInstance);
    }
}

public interface IDeserializer : IDisposable
{
    /// <summary>
    /// Read a value of any type, as decided by the input. This method can be used when the
    /// expected type may vary at runtime, such as when reading a union type.
    /// </summary>
    /// <remarks>
    /// Note that `T` is constrained to `class` to avoid GVM size explosion with AOT.
    /// </remarks>
    T ReadAny<T>(IDeserializeVisitor<T> v)
        where T : class;

    T? ReadNullableRef<T, D>(D deserialize)
        where T : class
        where D : IDeserialize<T>;

    bool ReadBool();
    char ReadChar();
    byte ReadByte();
    ushort ReadU16();
    uint ReadU32();
    ulong ReadU64();
    sbyte ReadSByte();
    short ReadI16();
    int ReadI32();
    long ReadI64();
    float ReadFloat();
    double ReadDouble();
    decimal ReadDecimal();
    string ReadString();
    IDeserializeCollection ReadCollection(ISerdeInfo typeInfo);
    IDeserializeType ReadType(ISerdeInfo typeInfo);
}