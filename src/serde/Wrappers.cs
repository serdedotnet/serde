// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Serde;

public readonly struct IdWrap<T> : ISerialize<T>
    where T : ISerialize<T>
{
    public static ISerdeInfo SerdeInfo => SerdeInfoProvider.GetInfo<T>();
    void ISerialize<T>.Serialize(T value, ISerializer serializer) => value.Serialize(value, serializer);
}

public readonly partial record struct BoolWrap
    : ISerialize<bool>, IDeserialize<bool>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "bool";
    void ISerialize<bool>.Serialize(bool value, ISerializer serializer)
        => serializer.SerializeBool(value);
    static bool IDeserialize<bool>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadBool();
    }
}

public readonly partial struct CharWrap
    : ISerialize<char>, IDeserialize<char>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);

    private const string s_typeName = "char";
    void ISerialize<char>.Serialize(char value, ISerializer serializer)
        => serializer.SerializeChar(value);
    static char IDeserialize<char>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadChar();
    }
}

public readonly partial struct ByteWrap
    : ISerialize<byte>, IDeserialize<byte>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "byte";
    public void Serialize(byte value, ISerializer serializer)
    {
        serializer.SerializeByte(value);
    }
    static byte IDeserialize<byte>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadByte();
    }
}

public readonly partial struct UInt16Wrap
    : ISerialize<ushort>, IDeserialize<ushort>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "ushort";
    void ISerialize<ushort>.Serialize(ushort value, ISerializer serializer)
    {
        serializer.SerializeU16(value);
    }
    static ushort IDeserialize<ushort>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadU16();
    }
}

public readonly partial struct UInt32Wrap
    : ISerialize<uint>, IDeserialize<uint>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "uint";
    void ISerialize<uint>.Serialize(uint value, ISerializer serializer)
        => serializer.SerializeU32(value);
    static uint IDeserialize<uint>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadU32();
    }
}

public readonly partial struct UInt64Wrap
    : ISerialize<ulong>, IDeserialize<ulong>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "ulong";
    void ISerialize<ulong>.Serialize(ulong value, ISerializer serializer)
        => serializer.SerializeU64(value);
    static ulong IDeserialize<ulong>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadU64();
    }
}

public readonly partial struct SByteWrap
    : ISerialize<sbyte>, IDeserialize<sbyte>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "sbyte";
    void ISerialize<sbyte>.Serialize(sbyte value, ISerializer serializer)
        => serializer.SerializeSByte(value);
    static sbyte IDeserialize<sbyte>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadSByte();
    }
}

public readonly partial struct Int16Wrap
    : ISerialize<short>, IDeserialize<short>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "short";
    void ISerialize<short>.Serialize(short value, ISerializer serializer)
    {
        serializer.SerializeI16(value);
    }
    static short IDeserialize<short>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadI16();
    }
}

public readonly partial struct Int32Wrap
    : ISerialize<int>, IDeserialize<int>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "int";
    void ISerialize<int>.Serialize(int value, ISerializer serializer)
    {
        serializer.SerializeI32(value);
    }
    static int IDeserialize<int>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadI32();
    }
}

public readonly partial struct Int64Wrap
    : ISerialize<long>, IDeserialize<long>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "long";
    void ISerialize<long>.Serialize(long value, ISerializer serializer)
        => serializer.SerializeI64(value);
    static long IDeserialize<long>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadI64();
    }

    private sealed class SerdeVisitor : IDeserializeVisitor<long>
    {
        public static readonly SerdeVisitor Instance = new SerdeVisitor();
        public string ExpectedTypeName => s_typeName;
        long IDeserializeVisitor<long>.VisitByte(byte b)    => b;
        long IDeserializeVisitor<long>.VisitU16(ushort u16) => u16;
        long IDeserializeVisitor<long>.VisitU32(uint u32)   => u32;
        long IDeserializeVisitor<long>.VisitU64(ulong u64)  => Convert.ToInt64(u64);
        long IDeserializeVisitor<long>.VisitSByte(sbyte b)  => b;
        long IDeserializeVisitor<long>.VisitI16(short i16)  => i16;
        long IDeserializeVisitor<long>.VisitI32(int i32)    => i32;
        long IDeserializeVisitor<long>.VisitI64(long i64)   => Convert.ToInt64(i64);
    }
}

public readonly record struct SingleWrap : ISerialize<float>, IDeserialize<float>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("float");
    public void Serialize(float value, ISerializer serializer)
        => serializer.SerializeFloat(value);
    public static float Deserialize(IDeserializer deserializer)
        => deserializer.ReadFloat();
}

public readonly partial record struct DoubleWrap
    : ISerialize<double>, IDeserialize<double>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("double");
    void ISerialize<double>.Serialize(double value, ISerializer serializer)
    {
        serializer.SerializeDouble(value);
    }
    static double IDeserialize<double>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadDouble();
    }
}

public readonly partial struct DecimalWrap
    : ISerialize<decimal>, IDeserialize<decimal>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("decimal");
    void ISerialize<decimal>.Serialize(decimal value, ISerializer serializer)
    {
        serializer.SerializeDecimal(value);
    }
    static decimal IDeserialize<decimal>.Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadDecimal();
    }
}
public readonly partial struct StringWrap
    : ISerialize<string>, IDeserialize<string>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName);
    private const string s_typeName = "string";
    void ISerialize<string>.Serialize(string value, ISerializer serializer)
    {
        serializer.SerializeString(value);
    }
    public static string Deserialize(IDeserializer deserializer)
    {
        return deserializer.ReadString();
    }
}

/// <summary>
/// Represents a nullable type (struct or class). In Serde the nullable type wrappers are unified
/// into a single wrapper. They each have a single field named "Value" which is the wrapped type and
/// their type name is the wrapped type name followed by '?'.
/// </summary>
file sealed record NullableSerdeInfo<TInfoProvider> : ISerdeInfo
    where TInfoProvider : ISerdeInfoProvider
{
    public static readonly NullableSerdeInfo<TInfoProvider> Instance = new();

    public string Name { get; }
    public ISerdeInfo WrappedInfo { get; }

    private NullableSerdeInfo()
    {
        WrappedInfo = TInfoProvider.SerdeInfo;
        Name = WrappedInfo.Name + "?";
    }

    public InfoKind Kind => InfoKind.CustomType;
    public int FieldCount => 1;

    public IList<CustomAttributeData> Attributes => [];

    public Utf8Span GetFieldName(int index)
        => index == 0 ? "Value"u8 : throw GetOOR(index);

    public string GetFieldStringName(int index)
        => index == 0 ? "Value" : throw GetOOR(index);

    public IList<CustomAttributeData> GetFieldAttributes(int index)
        => index == 0 ? [] : throw GetOOR(index);

    public int TryGetIndex(Utf8Span fieldName) => fieldName == "Value"u8 ? 0 : IDeserializeType.IndexNotFound;

    public ISerdeInfo GetFieldInfo(int index) => index == 0 ? WrappedInfo : throw GetOOR(index);

    private ArgumentOutOfRangeException GetOOR(int index)
        => new ArgumentOutOfRangeException(nameof(index), index, $"{Name} has only one field.");
}


public static class NullableWrap
{
    public readonly partial record struct SerializeImpl<T, TWrap> : ISerialize<T?>
        where T : struct
        where TWrap : struct, ISerialize<T>
    {
        public static ISerdeInfo SerdeInfo => NullableSerdeInfo<TWrap>.Instance;
        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is {} notnull)
            {
                default(TWrap).Serialize(notnull, serializer);
            }
            else
            {
                serializer.SerializeNull();
            }
        }
    }

    public readonly partial record struct DeserializeImpl<T, TWrap>
        : IDeserialize<T?>
        where T : struct
        where TWrap : IDeserialize<T>
    {
        public static ISerdeInfo SerdeInfo => NullableSerdeInfo<TWrap>.Instance;
        public static T? Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef(new Visitor());
        }

        private sealed class Visitor : IDeserializeVisitor<T?>
        {
            public string ExpectedTypeName => typeof(T).ToString() + "?";

            T? IDeserializeVisitor<T?>.VisitNull()
            {
                return null;
            }

            T? IDeserializeVisitor<T?>.VisitNotNull(IDeserializer d)
            {
                return TWrap.Deserialize(d);
            }
        }
    }
}

public static class NullableRefWrap
{
    public readonly partial record struct SerializeImpl<T, TWrap> : ISerialize<T?>
        where T : class
        where TWrap : struct, ISerialize<T>
    {
        public static ISerdeInfo SerdeInfo => NullableSerdeInfo<TWrap>.Instance;

        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is null)
            {
                serializer.SerializeNull();
            }
            else
            {
                default(TWrap).Serialize(value, serializer);
            }
        }
    }

    public readonly partial record struct DeserializeImpl<T, TWrap>(T? Value)
        : IDeserialize<T?>
        where T : class
        where TWrap : IDeserialize<T>
    {
        public static ISerdeInfo SerdeInfo => NullableSerdeInfo<TWrap>.Instance;

        public static T? Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef(new Visitor());
        }

        private sealed class Visitor : IDeserializeVisitor<T?>
        {
            public string ExpectedTypeName => typeof(T).ToString() + "?";

            T? IDeserializeVisitor<T?>.VisitNull()
            {
                return null;
            }

            T? IDeserializeVisitor<T?>.VisitNotNull(IDeserializer d)
            {
                return TWrap.Deserialize(d);
            }
        }
    }
}