
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
        private static readonly ISerdeInfo s_typeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray<int>).ToString(),
            InfoKind.Enumerable);
        public readonly struct SerializeImpl<T, TWrap> : ISerialize<CustomImArray<T>>
            where TWrap : struct, ISerialize<T>
        {
            public static ISerdeInfo SerdeInfo => s_typeInfo;
            void ISerialize<CustomImArray<T>>.Serialize(CustomImArray<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(s_typeInfo, value.Backing.AsSpan(), serializer);
        }
        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<CustomImArray<T>>
            where TWrap : IDeserialize<T>
        {
            public static ISerdeInfo SerdeInfo => s_typeInfo;
            public static CustomImArray<T> Deserialize(IDeserializer deserializer)
            {
                var serdeInfo = s_typeInfo;
                ImmutableArray<T>.Builder builder;
                var d = deserializer.ReadCollection(serdeInfo);
                if (d.SizeOpt is int size)
                {
                    builder = ImmutableArray.CreateBuilder<T>(size);
                }
                else
                {
                    size = -1; // Set initial size to unknown
                    builder = ImmutableArray.CreateBuilder<T>();
                }

                while (d.TryReadValue<T, TWrap>(serdeInfo, out T? next))
                {
                    builder.Add(next);
                }
                if (size >= 0 && builder.Count != size)
                {
                    throw DeserializeException.WrongItemCount(size, builder.Count);
                }
                return new CustomImArray<T>(builder.ToImmutable());
            }
        }
    }

    internal static partial class CustomImArray2Wrap
    {
        private static readonly ISerdeInfo s_typeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray2<int>).ToString(),
            InfoKind.Enumerable);

        public readonly record struct SerializeImpl<T, TWrap> : ISerialize<CustomImArray2<T>>
            where TWrap : struct, ISerialize<T>
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_typeInfo;

            void ISerialize<CustomImArray2<T>>.Serialize(CustomImArray2<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(s_typeInfo, value.Backing.AsSpan(), serializer);
        }
        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<CustomImArray2<T>>
            where TWrap : IDeserialize<T>
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_typeInfo;

            public static CustomImArray2<T> Deserialize(IDeserializer deserializer)
            {
                ImmutableArray<T>.Builder builder;
                var typeInfo = s_typeInfo;
                var d = deserializer.ReadCollection(typeInfo);
                if (d.SizeOpt is int size)
                {
                    builder = ImmutableArray.CreateBuilder<T>(size);
                }
                else
                {
                    size = -1; // Set initial size to unknown
                    builder = ImmutableArray.CreateBuilder<T>();
                }

                while (d.TryReadValue<T, TWrap>(typeInfo, out T? next))
                {
                    builder.Add(next);
                }
                if (size >= 0 && builder.Count != size)
                {
                    throw DeserializeException.WrongItemCount(size, builder.Count);
                }
                return new CustomImArray2<T>(builder.ToImmutable());
            }
        }
    }
}