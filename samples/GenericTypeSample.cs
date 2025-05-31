
using System;
using System.Collections.Immutable;
using System.Linq;
using Serde;
using Serde.Json;

namespace EqArraySample;

[SerdeTypeOptions(Proxy = typeof(EqArrayProxy))]
public readonly struct EqArray<T>(ImmutableArray<T> value) : IEquatable<EqArray<T>>
{
    public ImmutableArray<T> Array => value;

    public override bool Equals(object? obj) => obj is EqArray<T> other && Equals(other);
    public bool Equals(EqArray<T> other) => value.SequenceEqual(other.Array);

    public static bool operator ==(EqArray<T> left, EqArray<T> right) => left.Equals(right);
    public static bool operator !=(EqArray<T> left, EqArray<T> right) => !(left == right);

    public override string ToString() => $"[ {string.Join(", ", value.Select(item => item?.ToString() ?? "null"))} ]";

    public override int GetHashCode()
    => value.Aggregate(0, (hash, item) => HashCode.Combine(hash, item?.GetHashCode() ?? 0));
}

public static class EqArrayProxy
{
    private static readonly ISerdeInfo s_typeInfo = Serde.SerdeInfo.MakeEnumerable("EqArray");
    public sealed class Ser<T, TProvider> : ISerializeProvider<EqArray<T>>, ISerialize<EqArray<T>>
        where TProvider : ISerializeProvider<T>
    {
        public static readonly Ser<T, TProvider> Instance = new();
        static ISerialize<EqArray<T>> ISerializeProvider<EqArray<T>>.Instance => Instance;

        public ISerdeInfo SerdeInfo => s_typeInfo;

        void ISerialize<EqArray<T>>.Serialize(EqArray<T> value, ISerializer serializer)
        {
            ImmutableArrayProxy.Ser<T, TProvider>.Instance.Serialize(value.Array, serializer);
        }
    }

    public sealed class De<T, TProvider> : IDeserializeProvider<EqArray<T>>, IDeserialize<EqArray<T>>
        where TProvider : IDeserializeProvider<T>
    {
        public static readonly De<T, TProvider> Instance = new();
        static IDeserialize<EqArray<T>> IDeserializeProvider<EqArray<T>>.Instance => Instance;

        public ISerdeInfo SerdeInfo => s_typeInfo;
        EqArray<T> IDeserialize<EqArray<T>>.Deserialize(IDeserializer deserializer)
        {
            return new(ImmutableArrayProxy.De<T, TProvider>.Instance.Deserialize(deserializer));
        }
    }
}

public class GenericTypeSample
{
    public static void Run()
    {
        var eq = new EqArray<int>([ 1, 2, 3, 4 ]);
        Console.WriteLine($"Original version: {eq}");

        // Serialize the version to a JSON string
        // Unlike in regular custom serialization, we can't attach the serde object to
        // the Version type directly, so we have to pass it in explicitly.
        var json = JsonSerializer.Serialize<EqArray<int>, EqArrayProxy.Ser<int, I32Proxy>>(eq);
        Console.WriteLine($"Serialized version: {json}");

        // Deserialize the JSON string back to a Version object
        var deEq = JsonSerializer.Deserialize<EqArray<int>, EqArrayProxy.De<int, I32Proxy>>(json);
        if (!eq.Equals(deEq))
        {
            throw new InvalidOperationException("Deserialized version does not match the original.");
        }
        Console.WriteLine($"Deserialized version: {deEq}");
    }
}