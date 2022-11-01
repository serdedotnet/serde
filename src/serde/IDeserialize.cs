
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Serde
{
    public interface IDeserialize<T>
    {
        abstract static T Deserialize<D>(ref D deserializer)
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
        T VisitEnumerable<D>(ref D d) where D : IDeserializeEnumerable
            => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitDictionary<D>(ref D d) where D : IDeserializeDictionary
            => throw new InvalidDeserializeValueException("Expected type " + ExpectedTypeName);
        T VisitNull() => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
        T VisitNotNull<D>(ref D d) where D : IDeserializer => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
    }

    /// <summary>
    /// The stateful version of <see cref="IDeserialize{T}" />.
    /// </summary>
    public interface IDeserializeStateful<T>
    {
        T Deserialize<D>(ref D deserializer)
            where D : IDeserializer;
    }

    public interface IDeserializeEnumerable
    {
        bool TryGetNextStateful<T, D>(D d, [MaybeNullWhen(false)] out T next)
            where D : IDeserializeStateful<T>;
        int? SizeOpt { get; }
    }

    public interface IDeserializeDictionary
    {
        bool TryGetNextKeyStateful<K, D>(D d, [MaybeNullWhen(false)] out K next)
            where D : IDeserializeStateful<K>;
        V GetNextValueStateful<V, D>(D d) where D : IDeserializeStateful<V>;
        bool TryGetNextEntryStateful<K, DK, V, DV>(DK dk, DV dv, [MaybeNullWhen(false)] out (K, V) next)
            where DK : IDeserializeStateful<K>
            where DV : IDeserializeStateful<V>;
        int? SizeOpt { get; }
    }

    public static class DeserializeStatefulExt
    {
        public struct DeserializeForward<T, DT> : IDeserializeStateful<T>
            where DT : IDeserialize<T>
        {
            T IDeserializeStateful<T>.Deserialize<D>(ref D deserializer)
            {
                return DT.Deserialize(ref deserializer);
            }
        }

        public static bool TryGetNext<TEnum, T, D>(this TEnum e, [MaybeNullWhen(false)] out T next)
            where TEnum : IDeserializeEnumerable
            where D : IDeserialize<T>
        {
            return e.TryGetNextStateful(new DeserializeForward<T, D>(), out next);
        }

        public static bool TryGetNextKey<TDict, T, D>(this TDict dict, [MaybeNullWhen(false)] out T next)
            where TDict : IDeserializeDictionary
            where D : IDeserialize<T>
        {
            return dict.TryGetNextKeyStateful<T, DeserializeForward<T, D>>(new DeserializeForward<T, D>(), out next);
        } 

        public static V GetNextValue<TDict, V, D>(this TDict dict)
            where TDict : IDeserializeDictionary
            where D : IDeserialize<V>
        {
            return dict.GetNextValueStateful<V, DeserializeForward<V, D>>(new DeserializeForward<V, D>());
        }

        public static bool TryGetNextEntry<TDict, K, DK, V, DV>(this TDict dict, [MaybeNullWhen(false)] out (K, V) next)
            where TDict : IDeserializeDictionary
            where DK : IDeserialize<K>
            where DV : IDeserialize<V>
        {
            return dict.TryGetNextEntryStateful<K, DeserializeForward<K, DK>, V, DeserializeForward<V, DV>>(
                new DeserializeForward<K, DK>(), new DeserializeForward<V, DV>(), out next);
        }
    }

    public interface IDeserializer
    {
        T DeserializeAny<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeBool<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeChar<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeByte<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeU16<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeU32<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeU64<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeSByte<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeI16<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeI32<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeI64<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeFloat<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeDouble<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeDecimal<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeString<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeIdentifier<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeType<T, V>(string typeName, ReadOnlySpan<string> fieldNames, V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeEnumerable<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeDictionary<T, V>(V v) where V : class, IDeserializeVisitor<T>;
        T DeserializeNullableRef<T, V>(V v) where V : class, IDeserializeVisitor<T>;
    }
}