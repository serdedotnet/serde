
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Serde
{
    /// <summary>
    /// The driving interface for deserializing a given type. This interface should be implemented
    /// for any type that wants to be deserialized by the Serde framework. The implementation should
    /// be independent of the format the type is being deserialized from.
    /// </summary>
    public interface IDeserialize<T>
    {
        abstract static T Deserialize<D>(ref D deserializer)
            where D : IDeserializer;
    }

    /// <summary>
    /// Thrown from implementations of <see cref="IDeserializer" />. Indicates that an unexpected
    /// value was seen in the input which cannot be converted to the target type.
    /// </summary>
    public sealed class InvalidDeserializeValueException : Exception
    {
        public InvalidDeserializeValueException(string msg)
        : base(msg)
        { }
    }

    public interface IDeserializeVisitor<T>
    {
        string ExpectedTypeName { get; }
        T VisitBool(bool b) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitChar(char c) => VisitString(c.ToString());
        T VisitByte(byte b) => VisitU64(b);
        T VisitU16(ushort u16) => VisitU64(u16);
        T VisitU32(uint u32) => VisitU64(u32);
        T VisitU64(ulong u64) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitSByte(sbyte b) => VisitI64(b);
        T VisitI16(short i16) => VisitI64(i16);
        T VisitI32(int i32) => VisitI64(i32);
        T VisitI64(long i64) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitFloat(float f) => VisitDouble(f);
        T VisitDouble(double d) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitDecimal(decimal d) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitString(string s) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitUtf8Span(ReadOnlySpan<byte> s) => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitEnumerable<D>(ref D d) where D : IDeserializeEnumerable
            => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitDictionary<D>(ref D d) where D : IDeserializeDictionary
            => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitNull() => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
        T VisitNotNull<D>(ref D d) where D : IDeserializer => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
    }

    public interface IDeserializeEnumerable
    {
        bool TryGetNext<T, D>([MaybeNullWhen(false)] out T next)
            where D : IDeserialize<T>;
        int? SizeOpt { get; }
    }

    public interface IDeserializeDictionary
    {
        bool TryGetNextKey<K, D>([MaybeNullWhen(false)] out K next)
            where D : IDeserialize<K>;
        V GetNextValue<V, D>() where D : IDeserialize<V>;
        bool TryGetNextEntry<K, DK, V, DV>([MaybeNullWhen(false)] out (K, V) next)
            where DK : IDeserialize<K>
            where DV : IDeserialize<V>;
        int? SizeOpt { get; }
    }

    public interface IDeserializer
    {
        T DeserializeAny<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeBool<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeChar<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeByte<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeU16<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeU32<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeU64<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeSByte<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeI16<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeI32<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeI64<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeFloat<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeDouble<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeDecimal<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeString<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeIdentifier<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeType<T, V>(string typeName, ReadOnlySpan<string> fieldNames, V v) where V : IDeserializeVisitor<T>;
        T DeserializeEnumerable<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeDictionary<T, V>(V v) where V : IDeserializeVisitor<T>;
        T DeserializeNullableRef<T, V>(V v) where V : IDeserializeVisitor<T>;
    }
}