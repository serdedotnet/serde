
using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace Serde
{
    public interface IDeserialize<T>
    {
        abstract static ValueTask<T> Deserialize<D>(D deserializer)
            where D : IDeserializer;
    }

    public sealed class InvalidDeserializeValueException : Exception
    {
        public InvalidDeserializeValueException(string msg)
        : base(msg)
        { }
    }

    public static class OptionExt
    {
        public static T GetValueOrThrow<T>(this Option<T> member, [CallerArgumentExpression("member")]string paramName = "")
        {
            return member.HasValue
                ? member.GetValueOrDefault()
                : throw new InvalidDeserializeValueException($"Required member '{paramName}' was missing a value in the source");
        }

        public static T GetValueOrDefault<T>(this Option<T> member, T defaultValue)
        {
            return member.HasValue
                ? member.GetValueOrDefault()
                : defaultValue;
        }
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
        T VisitNull() => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
        ValueTask<T> VisitEnumerable<D>(D d) where D : IDeserializeEnumerable
            => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        ValueTask<T> VisitDictionary<D>(D d) where D : IDeserializeDictionary
            => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        ValueTask<T> VisitNotNull<D>(D d) where D : IDeserializer => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
    }

    public interface IDeserializeEnumerable
    {
        ValueTask<Option<T>> TryGetNext<T, D>()
            where D : IDeserialize<T>;
        int? SizeOpt { get; }
    }

    public interface IDeserializeDictionary
    {
        ValueTask<Option<K>> TryGetNextKey<K, D>() where D : IDeserialize<K>;
        ValueTask<V> GetNextValue<V, D>() where D : IDeserialize<V>;
        ValueTask<Option<(K, V)>> TryGetNextEntry<K, DK, V, DV>()
            where DK : IDeserialize<K>
            where DV : IDeserialize<V>;
        int? SizeOpt { get; }
    }

    public interface IDeserializer
    {
        ValueTask<T> DeserializeAny<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeBool<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeChar<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeByte<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeU16<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeU32<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeU64<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeSByte<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeI16<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeI32<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeI64<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeFloat<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeDouble<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeDecimal<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeString<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeIdentifier<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeType<T, V>(string typeName, ReadOnlySpan<string> fieldNames, V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeEnumerable<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeDictionary<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        ValueTask<T> DeserializeNullableRef<T, V>(V v) where V : class, IDeserializeVisitor<T>;
    }
}