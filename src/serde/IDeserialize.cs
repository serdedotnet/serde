
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Serde
{
    /// <summary>
    /// The driving interface for deserializing a given type. This interface should be implemented
    /// for any type that wants to be deserialized by the Serde framework. The implementation should
    /// be independent of the format the type is being deserialized from.
    /// </summary>
    public interface IDeserialize<T>
    {
        abstract static T Deserialize(IDeserializer deserializer);
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
        T VisitNotNull(IDeserializer d) => throw new InvalidOperationException("Expected type " + ExpectedTypeName);
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

    public interface IDeserializeType
    {
        public const int EndOfType = -1;
        public const int IndexNotFound = -2;

        int TryReadIndex(FieldMap map);

        V ReadValue<V, D>() where D : IDeserialize<V>;
    }

    /// <summary>
    /// A map from field names to int indices. This is an optimization for deserializing types
    /// that avoids allocating strings for field names.
    /// </summary>
    public sealed class FieldMap
    {
        #region Fields and auto-props

        public string TypeName { get; }
        private readonly ImmutableArray<(byte[] Utf8String, int Index)> _fieldNames;

        #endregion


        private static readonly UTF8Encoding s_utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        /// <summary>
        /// Create a new field mapping. The ordering of the field names is important -- it
        /// corresponds to the index returned by <see cref="IDeserializeType.TryReadIndex" />.
        /// </summary>
        public FieldMap(
            string typeName,
            ReadOnlySpan<string> fieldNames)
        {
            TypeName = typeName;

            var builder = ImmutableArray.CreateBuilder<(byte[] Utf8String, int Index)>(fieldNames.Length);
            for (int index = 0; index < fieldNames.Length; index++)
            {
                builder.Add((s_utf8.GetBytes(fieldNames[index]), index));
            }
            builder.Sort((left, right) =>
                left.Utf8String.AsSpan().SequenceCompareTo(right.Utf8String.AsSpan()));

            _fieldNames = builder.MoveToImmutable();
        }

        public int TryReadIndex(Utf8Span utf8FieldName)
        {
            int mapIndex = BinarySearch(_fieldNames.AsSpan(), utf8FieldName);

            return mapIndex < 0 ? IDeserializeType.IndexNotFound : _fieldNames[mapIndex].Index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BinarySearch(ReadOnlySpan<(byte[] Utf8String, int Index)> span, Utf8Span fieldName)
        {
            return BinarySearch(ref MemoryMarshal.GetReference(span), span.Length, fieldName);
        }

        // This is a copy of the BinarySearch method from System.MemoryExtensions.
        // We can't use that version because ref structs can't yet be substituted for type arguments.
        private static int BinarySearch(ref (byte[] Utf8String, int Index) spanStart, int length, Utf8Span fieldName)
        {
            int lo = 0;
            int hi = length - 1;
            // If length == 0, hi == -1, and loop will not be entered
            while (lo <= hi)
            {
                // PERF: `lo` or `hi` will never be negative inside the loop,
                //       so computing median using uints is safe since we know
                //       `length <= int.MaxValue`, and indices are >= 0
                //       and thus cannot overflow an uint.
                //       Saves one subtraction per loop compared to
                //       `int i = lo + ((hi - lo) >> 1);`
                int i = (int)(((uint)hi + (uint)lo) >> 1);

                int c = fieldName.SequenceCompareTo(Unsafe.Add(ref spanStart, i).Utf8String);
                if (c == 0)
                {
                    return i;
                }
                else if (c > 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }
            // If none found, then a negative number that is the bitwise complement
            // of the index of the next element that is larger than or, if there is
            // no larger element, the bitwise complement of `length`, which
            // is `lo` at this point.
            return ~lo;
        }
    }

    public interface IDeserializer
    {
        T DeserializeAny<T>(IDeserializeVisitor<T> v);
        T DeserializeBool<T>(IDeserializeVisitor<T> v);
        T DeserializeChar<T>(IDeserializeVisitor<T> v);
        T DeserializeByte<T>(IDeserializeVisitor<T> v);
        T DeserializeU16<T>(IDeserializeVisitor<T> v);
        T DeserializeU32<T>(IDeserializeVisitor<T> v);
        T DeserializeU64<T>(IDeserializeVisitor<T> v);
        T DeserializeSByte<T>(IDeserializeVisitor<T> v);
        T DeserializeI16<T>(IDeserializeVisitor<T> v);
        T DeserializeI32<T>(IDeserializeVisitor<T> v);
        T DeserializeI64<T>(IDeserializeVisitor<T> v);
        T DeserializeFloat<T>(IDeserializeVisitor<T> v);
        T DeserializeDouble<T>(IDeserializeVisitor<T> v);
        T DeserializeDecimal<T>(IDeserializeVisitor<T> v);
        T DeserializeString<T>(IDeserializeVisitor<T> v);
        T DeserializeIdentifier<T>(IDeserializeVisitor<T> v);
        T DeserializeType<T>(string typeName, ReadOnlySpan<string> fieldNames, IDeserializeVisitor<T> v);
        T DeserializeEnumerable<T>(IDeserializeVisitor<T> v);
        T DeserializeDictionary<T>(IDeserializeVisitor<T> v);
        T DeserializeNullableRef<T>(IDeserializeVisitor<T> v);
        IDeserializeType DeserializeType(FieldMap fieldMap);
    }
}