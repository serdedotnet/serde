
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
            InfoKind.List);

        public sealed class Ser<T, TProvider>()
            : SerListBase<Ser<T, TProvider>, T, CustomImArray<T>, TProvider>(s_serdeInfo),
              ISerializeProvider<CustomImArray<T>>
            where TProvider : ISerializeProvider<T>
        {
            protected override ReadOnlySpan<T> GetSpan(CustomImArray<T> value) => value.Backing.AsSpan();
        }

        public sealed class De<T, TProvider>()
            : DeListBase<De<T, TProvider>, T, CustomImArray<T>, ImmutableArray<T>.Builder, TProvider>(s_serdeInfo)
            where TProvider : IDeserializeProvider<T>
        {
            protected override ImmutableArray<T>.Builder GetBuilder(int? sizeOpt)
            {
                return sizeOpt is int size
                    ? ImmutableArray.CreateBuilder<T>(size)
                    : ImmutableArray.CreateBuilder<T>();
            }

            protected override CustomImArray<T> ToList(ImmutableArray<T>.Builder builder)
            {
                return new CustomImArray<T>(builder.ToImmutable());
            }
        }
    }

    internal static partial class CustomImArray2Proxy
    {
        private static readonly ISerdeInfo s_serdeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray2<int>).ToString(),
            InfoKind.List);

        public sealed class Ser<T, TProvider>()
            : SerListBase<Ser<T, TProvider>, T, CustomImArray2<T>, TProvider>(s_serdeInfo),
              ISerializeProvider<CustomImArray2<T>>
            where T : notnull
            where TProvider : ISerializeProvider<T>
        {
            protected override ReadOnlySpan<T> GetSpan(CustomImArray2<T> value)
            {
                return value.Backing.AsSpan();
            }
        }

        public sealed class De<T, TProvider>()
            : DeListBase<De<T, TProvider>, T, CustomImArray2<T>, ImmutableArray<T>.Builder, TProvider>(s_serdeInfo)
            where TProvider : IDeserializeProvider<T>
        {
            protected override ImmutableArray<T>.Builder GetBuilder(int? sizeOpt)
            {
                return sizeOpt is int size
                    ? ImmutableArray.CreateBuilder<T>(size)
                    : ImmutableArray.CreateBuilder<T>();
            }

            protected override CustomImArray2<T> ToList(ImmutableArray<T>.Builder builder)
            {
                return new CustomImArray2<T>(builder.ToImmutable());
            }
        }
    }
}