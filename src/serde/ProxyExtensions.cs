
#if NET10_0_OR_GREATER
using System.Collections.Generic;

namespace Serde;

public static class BoolExtensions
{
    extension(bool)
    {
        public static ISerialize<bool> Serialize => BoolProxy.Instance;
        public static IDeserialize<bool> Deserialize => BoolProxy.Instance;
    }
}

public static class CharExtensions
{
    extension(char)
    {
        public static ISerialize<char> Serialize => CharProxy.Instance;
        public static IDeserialize<char> Deserialize => CharProxy.Instance;
    }
}

public static class ByteExtensions
{
    extension(byte)
    {
        public static ISerialize<byte> Serialize => U8Proxy.Instance;
        public static IDeserialize<byte> Deserialize => U8Proxy.Instance;
    }
}

public static class UInt16Extensions
{
    extension(ushort)
    {
        public static ISerialize<ushort> Serialize => U16Proxy.Instance;
        public static IDeserialize<ushort> Deserialize => U16Proxy.Instance;
    }
}

public static class UInt32Extensions
{
    extension(uint)
    {
        public static ISerialize<uint> Serialize => U32Proxy.Instance;
        public static IDeserialize<uint> Deserialize => U32Proxy.Instance;
    }
}

public static class UInt64Extensions
{
    extension(ulong)
    {
        public static ISerialize<ulong> Serialize => U64Proxy.Instance;
        public static IDeserialize<ulong> Deserialize => U64Proxy.Instance;
    }
}

public static class SByteExtensions
{
    extension(sbyte)
    {
        public static ISerialize<sbyte> Serialize => I8Proxy.Instance;
        public static IDeserialize<sbyte> Deserialize => I8Proxy.Instance;
    }
}

public static class Int16Extensions
{
    extension(short)
    {
        public static ISerialize<short> Serialize => I16Proxy.Instance;
        public static IDeserialize<short> Deserialize => I16Proxy.Instance;
    }
}

public static class Int32Extensions
{
    extension(int)
    {
        public static ISerialize<int> Serialize => I32Proxy.Instance;
        public static IDeserialize<int> Deserialize => I32Proxy.Instance;
    }
}

public static class Int64Extensions
{
    extension(long)
    {
        public static ISerialize<long> Serialize => I64Proxy.Instance;
        public static IDeserialize<long> Deserialize => I64Proxy.Instance;
    }
}

public static class StringExtensions
{
    extension(string)
    {
        public static ISerialize<string> Serialize => StringProxy.Instance;
        public static IDeserialize<string> Deserialize => StringProxy.Instance;
    }
}

public static class FloatExtensions
{
    extension(float)
    {
        public static ISerialize<float> Serialize => F32Proxy.Instance;
        public static IDeserialize<float> Deserialize => F32Proxy.Instance;
    }
}

public static class DoubleExtensions
{
    extension(double)
    {
        public static ISerialize<double> Serialize => F64Proxy.Instance;
        public static IDeserialize<double> Deserialize => F64Proxy.Instance;
    }
}

public static class DecimalExtensions
{
    extension(decimal)
    {
        public static ISerialize<decimal> Serialize => DecimalProxy.Instance;
        public static IDeserialize<decimal> Deserialize => DecimalProxy.Instance;
    }
}

public static class ArrayTExtensions
{
    extension<T>(T[]) where T : IDeserializeProvider<T>
    {
        public static IDeserialize<T[]> Deserialize => ArrayProxy.De<T, T>.Instance;
    }
    extension<T>(T[]) where T : ISerializeProvider<T>
    {
        public static ISerialize<T[]> Serialize => ArrayProxy.Ser<T, T>.Instance;
    }
}

public static class ArrayIntExtensions
{
    extension(int[])
    {
        public static IDeserialize<int[]> Deserialize => ArrayProxy.De<int, I32Proxy>.Instance;
    }
    extension(int[])
    {
        public static ISerialize<int[]> Serialize => ArrayProxy.Ser<int, I32Proxy>.Instance;
    }
}

public static class ArrayStringExtensions
{
    extension(string[])
    {
        public static IDeserialize<string[]> Deserialize => ArrayProxy.De<string, StringProxy>.Instance;
    }
    extension(string[])
    {
        public static ISerialize<string[]> Serialize => ArrayProxy.Ser<string, StringProxy>.Instance;
    }
}

public static class ListTExtensions
{
    extension<T>(List<T>) where T : IDeserializeProvider<T>
    {
        public static IDeserialize<List<T>> Deserialize => ListProxy.De<T, T>.Instance;
    }
    extension<T>(List<T>) where T : ISerializeProvider<T>
    {
        public static ISerialize<List<T>> Serialize => ListProxy.Ser<T, T>.Instance;
    }
}
public static class ListIntExtensions
{
    extension(List<int>)
    {
        public static IDeserialize<List<int>> Deserialize => ListProxy.De<int, I32Proxy>.Instance;
    }
    extension(List<int>)
    {
        public static ISerialize<List<int>> Serialize => ListProxy.Ser<int, I32Proxy>.Instance;
    }
}
public static class ListStringExtensions
{
    extension(List<string>)
    {
        public static IDeserialize<List<string>> Deserialize => ListProxy.De<string, StringProxy>.Instance;
    }
    extension(List<string>)
    {
        public static ISerialize<List<string>> Serialize => ListProxy.Ser<string, StringProxy>.Instance;
    }
}
public static class DictStringTExtensions
{
    extension<T>(Dictionary<string, T>) where T : IDeserializeProvider<T>
    {
        public static IDeserialize<Dictionary<string, T>> Deserialize => DictProxy.De<string, T, StringProxy, T>.Instance;
    }
    extension<T>(Dictionary<string, T>) where T : ISerializeProvider<T>
    {
        public static ISerialize<Dictionary<string, T>> Serialize => DictProxy.Ser<string, T, StringProxy, T>.Instance;
    }
}
public static class DictStringStringExtensions
{
    extension(Dictionary<string, string?>)
    {
        public static IDeserialize<Dictionary<string, string?>> Deserialize => DictProxy.De<string, string?, StringProxy, NullableRefProxy.De<string, StringProxy>>.Instance;
    }
    extension(Dictionary<string, string?>)
    {
        public static ISerialize<Dictionary<string, string>> Serialize => DictProxy.Ser<string, string, StringProxy, StringProxy>.Instance;
    }
}
public static class DictStringNullableStringExtensions
{
    extension(Dictionary<string, string>)
    {
        public static IDeserialize<Dictionary<string, string>> Deserialize => DictProxy.De<string, string, StringProxy, StringProxy>.Instance;
    }
    extension(Dictionary<string, string?>)
    {
        public static ISerialize<Dictionary<string, string?>> Serialize => DictProxy.Ser<string, string?, StringProxy, NullableRefProxy.Ser<string, StringProxy>>.Instance;
    }
}

#endif