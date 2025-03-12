
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
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
        [SerdeMemberOptions(Proxy = typeof(CustomImArrayProxy))]
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

    [SerdeTypeOptions(Proxy = typeof(CustomImArray2Proxy))]
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

    internal static partial class CustomImArrayProxy
    {
        private static readonly ISerdeInfo s_serdeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray<int>).ToString(),
            InfoKind.Enumerable);

        public sealed class Ser<T, TProvider>
            : SerListBase<Ser<T, TProvider>, T, CustomImArray<T>, TProvider>,
              ISerializeProvider<CustomImArray<T>>
            where TProvider : ISerializeProvider<T>
        {
            public static ISerdeInfo SerdeInfo => s_serdeInfo;

            protected override ReadOnlySpan<T> GetSpan(CustomImArray<T> value) => value.Backing.AsSpan();
        }

        public sealed class De<T, TProvider> : IDeserialize<CustomImArray<T>>, IDeserializeProvider<CustomImArray<T>>
            where TProvider : IDeserializeProvider<T>
        {
            public static De<T, TProvider> Instance { get; } = new();
            static IDeserialize<CustomImArray<T>> IDeserializeProvider<CustomImArray<T>>.DeserializeInstance => Instance;
            public static ISerdeInfo SerdeInfo => s_serdeInfo;

            private readonly ITypeDeserialize<T> _proxy;

            private De()
            {
                var de = TProvider.DeserializeInstance;
                _proxy = de is ITypeDeserialize<T> typeDe
                    ? typeDe
                    : new TypeDeBoxed<T>(de);
            }

            CustomImArray<T> IDeserialize<CustomImArray<T>>.Deserialize(IDeserializer deserializer)
            {
                var serdeInfo = s_serdeInfo;
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

                int index;
                while ((index = d.TryReadIndex(serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                {
                    builder.Add(_proxy.Deserialize(d, serdeInfo, index));
                }
                if (size >= 0 && builder.Count != size)
                {
                    throw DeserializeException.WrongItemCount(size, builder.Count);
                }
                return new CustomImArray<T>(builder.ToImmutable());
            }
        }
    }

    internal static partial class CustomImArray2Proxy
    {
        private static readonly ISerdeInfo s_serdeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray2<int>).ToString(),
            InfoKind.Enumerable);

        public sealed class Ser<T, TProvider>
            : SerListBase<Ser<T, TProvider>, T, CustomImArray2<T>, TProvider>,
              ISerializeProvider<CustomImArray2<T>>
            where T : notnull
            where TProvider : ISerializeProvider<T>
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_serdeInfo;

            protected override ReadOnlySpan<T> GetSpan(CustomImArray2<T> value)
            {
                return value.Backing.AsSpan();
            }
        }

        public sealed class De<T, TProvider> : IDeserialize<CustomImArray2<T>>, IDeserializeProvider<CustomImArray2<T>>
            where TProvider : IDeserializeProvider<T>
        {
            public static De<T, TProvider> Instance { get; } = new();
            static IDeserialize<CustomImArray2<T>> IDeserializeProvider<CustomImArray2<T>>.DeserializeInstance => Instance;
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_serdeInfo;

            private readonly ITypeDeserialize<T> _de;

            private De()
            {
                var de = TProvider.DeserializeInstance;
                _de = de is ITypeDeserialize<T> typeDe
                    ? typeDe
                    : new TypeDeBoxed<T>(de);
            }

            public CustomImArray2<T> Deserialize(IDeserializer deserializer)
            {
                ImmutableArray<T>.Builder builder;
                var typeInfo = s_serdeInfo;
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

                int index;
                while ((index = d.TryReadIndex(typeInfo, out _)) != ITypeDeserializer.EndOfType)
                {
                    builder.Add(_de.Deserialize(d, typeInfo, index));
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