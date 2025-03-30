
namespace Serde;

/// <summary>
/// Convenience interface for implementing both serialization and deserialization.
/// See <see cref="ISerialize{T}"/> and <see cref="IDeserialize{T}"/> for details.
/// </summary>
public interface ISerde<T> : ISerialize<T>, IDeserialize<T> { }

/// <summary>
/// Convenience interface for implementing both serialization and deserialization providers.
/// See <see cref="ISerializeProvider{T}"/> and <see cref="IDeserializeProvider{T}"/> for details.
/// </summary>
public interface ISerdeProvider<TSelf, TSerde, T> : ISerializeProvider<T>, IDeserializeProvider<T>
    where TSelf : ISerdeProvider<TSelf, TSerde, T>
    where TSerde : ISerde<T>
{
    public new abstract static TSerde Instance { get; }
    static ISerialize<T> ISerializeProvider<T>.Instance => TSelf.Instance;
    static IDeserialize<T> IDeserializeProvider<T>.Instance => TSelf.Instance;
}