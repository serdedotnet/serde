using System;

namespace Serde;

/// <summary>
/// The driving interface for deserializing a given type. This interface separates deserialization
/// from the type being deserialized. This allows for deserialization to be implemented by a
/// different type than the one being deserialized. All underlying deserialization primitives should
/// be based on this interface. However, types which implement their own deserialization logic
/// should also implement <see cref="IDeserializeProvider{T}"/>.
/// </summary>
public interface IDeserialize<T> : ISerdeInfoProvider
{
    T Deserialize(IDeserializer deserializer);
    T DeserializeAsField(ITypeDeserializer typeDeserializer, ISerdeInfo serdeInfo, int index)
    {
        var d = typeDeserializer.ReadFieldStart(serdeInfo, index);
        var r = Deserialize(d);
        typeDeserializer.ReadFieldEnd(serdeInfo, index, d);
        return r;
    }
}

/// <summary>
/// Retrieves an instance of <see cref="IDeserialize{T}"/> for the given type <typeparamref name="T"/>.
/// This interface is used to attach a particular deserialization implementation to a type and make
/// it easier to discover that implementation.
/// </summary>
public interface IDeserializeProvider<T>
{
    static abstract IDeserialize<T> Instance { get; }
}

public static class DeserializeProvider
{
    public static IDeserialize<T> GetDeserialize<T>()
        where T : IDeserializeProvider<T> => T.Instance;

    public static IDeserialize<T> GetDeserialize<T, TProvider>()
        where TProvider : IDeserializeProvider<T> => TProvider.Instance;
}

/// <summary>
/// This is a perf optimization. It allows primitive types (and only primitive types) to be
/// deserialized without boxing. It is only useful for deserializing collections.
/// </summary>
[Obsolete("Use IDeserialize<T>.DeserializeAsField instead")]
public interface ITypeDeserialize<T> : IDeserialize<T>
{
    T Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index);
}

public static class TypeDeserialize
{
    [Obsolete("Use IDeserialize<T>.DeserializeAsField instead")]
    public static ITypeDeserialize<T> GetOrBox<T, TProvider>()
        where TProvider : IDeserializeProvider<T>
    {
        var d = TProvider.Instance;
        return d switch
        {
            ITypeDeserialize<T> typeDe => typeDe,
            _ => BoxProxy.De<T, TProvider>.Instance,
        };
    }
}
