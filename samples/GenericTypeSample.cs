
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Serde;
using Serde.Json;

namespace EqArraySample;

[SerdeTypeOptions(Proxy = typeof(EqArrayProxy))]
public readonly struct EqArray<T>(ImmutableArray<T> value)
{
    public ImmutableArray<T> Array => value;
}

public static class EqArrayProxy
{
    private static readonly ISerdeInfo s_typeInfo = Serde.SerdeInfo.MakeEnumerable("EqArray");
    public sealed class Ser<T, TProvider>
        : ISerializeProvider<EqArray<T>>, ISerialize<EqArray<T>>
        where TProvider : ISerializeProvider<T>
    {
        public static readonly Ser<T, TProvider> Instance = new();
        static ISerialize<EqArray<T>> ISerializeProvider<EqArray<T>>.Instance => Instance;

        public ISerdeInfo SerdeInfo => s_typeInfo;

        void ISerialize<EqArray<T>>.Serialize(EqArray<T> value, ISerializer serializer)
        {
            ImmutableArrayProxy.Ser<T, TProvider>.Instance.Serialize(
                value.Array,
                serializer
            );
        }
    }

    public sealed class De<T, TProvider> : IDeserializeProvider<EqArray<T>>, IDeserialize<EqArray<T>>
        where TProvider : IDeserializeProvider<T>
    {
        public static readonly De<T, TProvider> Instance = new();
        static IDeserialize<EqArray<T>> IDeserializeProvider<EqArray<T>>.Instance => Instance;

        public ISerdeInfo SerdeInfo => s_typeInfo;
        async Task<EqArray<T>> IDeserialize<EqArray<T>>.Deserialize(IDeserializer deserializer)
        {
            return new(await ImmutableArrayProxy.De<T, TProvider>.Instance.Deserialize(deserializer));
        }
    }
}

public class GenericTypeSample
{
    public static void Run()
    {
        var eq = new EqArray<int>([ 1, 2, 3, 4 ]);

        // Serialize the version to a JSON string.
        // Here we pass the proxy parameter explicitly, but if EqArray is a nested field, the
        // source generator will pass it automatically.
        var json = JsonSerializer.Serialize<EqArray<int>, EqArrayProxy.Ser<int, I32Proxy>>(eq);
        Console.WriteLine($"Serialized version: {json}");

        var deEq = JsonSerializer.Deserialize<EqArray<int>, EqArrayProxy.De<int, I32Proxy>>(json);
        if (!eq.Array.SequenceEqual(deEq.Array))
        {
            throw new InvalidOperationException("Deserialized version does not match the original.");
        }
    }
}