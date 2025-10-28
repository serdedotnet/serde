
#if NET10_0_OR_GREATER
using System.Collections.Generic;

namespace Serde;

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