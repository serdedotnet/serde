
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Xunit;

namespace Serde.Test;

public sealed partial class GenericWrapperTests
{
    internal readonly record struct CustomImArray<T>(ImmutableArray<T> Backing)
    {
        public bool Equals(CustomImArray<T> other)
        {
            return Backing.SequenceEqual(other.Backing);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in Backing)
            {
                hash = HashCode.Combine(hash, item);
            }
            return hash;
        }
    }

    [GenerateSerde]
    private partial record struct CustomImArrayExplicitWrapOnMember
    {
        [SerdeWrap(typeof(CustomImArrayWrap))]
        public CustomImArray<int> A;
    }

    [Fact]
    public void CustomEnumerableSerializeExplicitWrapOnMember()
    {
        var src = ImmutableArray.Create(new[] { 1, 2, 3 });
        var embed = new CustomImArrayExplicitWrapOnMember
        {
            A = new CustomImArray<int>(src)
        };
        var result = Serde.Json.JsonSerializer.Serialize(embed);
        Assert.Equal("""{"a":[1,2,3]}""", result);
        var deser = Serde.Json.JsonSerializer.Deserialize<CustomImArrayExplicitWrapOnMember>(result);
        Assert.Equal(embed, deser);
    }

    [SerdeWrap(typeof(CustomImArray2Wrap))]
    internal readonly record struct CustomImArray2<T>(ImmutableArray<T> Backing)
    {
        public bool Equals(CustomImArray2<T> other)
        {
            return Backing.SequenceEqual(other.Backing);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in Backing)
            {
                hash = HashCode.Combine(hash, item);
            }
            return hash;
        }

        public override string ToString()
        {
            return "[ " + string.Join(", ", Backing) + "]";
        }
    }

    [GenerateSerde]
    private partial record struct CustomArrayWrapExplicitOnType
    {
        public CustomImArray2<int> A;
    }

    [Fact]
    public void CustomEnumerableSerializeExplicitWrapOnType()
    {
        var src = ImmutableArray.Create(new[] { 1, 2, 3 });
        var embed = new CustomArrayWrapExplicitOnType
        {
            A = new CustomImArray2<int>(src)
        };
        var result = Serde.Json.JsonSerializer.Serialize(embed);
        Assert.Equal("""{"a":[1,2,3]}""", result);
        var deser = Serde.Json.JsonSerializer.Deserialize<CustomArrayWrapExplicitOnType>(result);
        Assert.Equal(embed, deser);
    }

    internal static partial class CustomImArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(CustomImArray<T> cia)
            : ISerialize, ISerialize<CustomImArray<T>>, ISerializeWrap<CustomImArray<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(CustomImArray<T> t) => new(t);

            void ISerialize<CustomImArray<T>>.Serialize(CustomImArray<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(CustomImArray<T>).ToString(), value.Backing.AsSpan(), serializer);
            void ISerialize.Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(CustomImArray<T>).ToString(), cia.Backing.AsSpan(), serializer);
        }
        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<CustomImArray<T>>
            where TWrap : IDeserialize<T>
        {
            public static CustomImArray<T> Deserialize(IDeserializer deserializer)
                => deserializer.DeserializeEnumerable(new Visitor());

            public struct Visitor : IDeserializeVisitor<CustomImArray<T>>
            {
                public string ExpectedTypeName => typeof(CustomImArray<T>).ToString();
                CustomImArray<T> IDeserializeVisitor<CustomImArray<T>>.VisitEnumerable<D>(ref D d)
                {
                    ImmutableArray<T>.Builder builder;
                    if (d.SizeOpt is int size)
                    {
                        builder = ImmutableArray.CreateBuilder<T>(size);
                    }
                    else
                    {
                        size = -1; // Set initial size to unknown
                        builder = ImmutableArray.CreateBuilder<T>();
                    }

                    while (d.TryGetNext<T, TWrap>(out T? next))
                    {
                        builder.Add(next);
                    }
                    if (size >= 0 && builder.Count != size)
                    {
                        throw new InvalidDeserializeValueException($"Expected {size} items, found {builder.Count}");
                    }
                    return new CustomImArray<T>(builder.ToImmutable());
                }
            }
        }
    }

    internal static partial class CustomImArray2Wrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(CustomImArray2<T> cia)
            : ISerialize, ISerialize<CustomImArray2<T>>, ISerializeWrap<CustomImArray2<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(CustomImArray2<T> t) => new(t);

            void ISerialize<CustomImArray2<T>>.Serialize(CustomImArray2<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(CustomImArray2<T>).ToString(), value.Backing.AsSpan(), serializer);
            void ISerialize.Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(CustomImArray2<T>).ToString(), cia.Backing.AsSpan(), serializer);
        }
        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<CustomImArray2<T>>
            where TWrap : IDeserialize<T>
        {
            public static CustomImArray2<T> Deserialize(IDeserializer deserializer)
                => deserializer.DeserializeEnumerable(new Visitor());

            public struct Visitor : IDeserializeVisitor<CustomImArray2<T>>
            {
                public string ExpectedTypeName => typeof(CustomImArray<T>).ToString();
                CustomImArray2<T> IDeserializeVisitor<CustomImArray2<T>>.VisitEnumerable<D>(ref D d)
                {
                    ImmutableArray<T>.Builder builder;
                    if (d.SizeOpt is int size)
                    {
                        builder = ImmutableArray.CreateBuilder<T>(size);
                    }
                    else
                    {
                        size = -1; // Set initial size to unknown
                        builder = ImmutableArray.CreateBuilder<T>();
                    }

                    while (d.TryGetNext<T, TWrap>(out T? next))
                    {
                        builder.Add(next);
                    }
                    if (size >= 0 && builder.Count != size)
                    {
                        throw new InvalidDeserializeValueException($"Expected {size} items, found {builder.Count}");
                    }
                    return new CustomImArray2<T>(builder.ToImmutable());
                }
            }
        }
    }
}