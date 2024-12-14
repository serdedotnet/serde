
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
        private static readonly ISerdeInfo s_typeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray<int>).ToString(),
            InfoKind.Enumerable);

        public sealed class Serialize<T, TProvider> : ISerialize<CustomImArray<T>>, ISerializeProvider<CustomImArray<T>>
            where TProvider : ISerializeProvider<T>
        {
            public static Serialize<T, TProvider> Instance { get; } = new();
            static ISerialize<CustomImArray<T>> ISerializeProvider<CustomImArray<T>>.SerializeInstance => Instance;
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_typeInfo;

            private readonly ISerialize<T> _proxy = TProvider.SerializeInstance;

            private Serialize() { }
            void ISerialize<CustomImArray<T>>.Serialize(CustomImArray<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan(s_typeInfo, value.Backing.AsSpan(), _proxy, serializer);
        }

        public sealed class Deserialize<T, TProvider> : IDeserialize<CustomImArray<T>>, IDeserializeProvider<CustomImArray<T>>
            where TProvider : IDeserializeProvider<T>
        {
            public static Deserialize<T, TProvider> Instance { get; } = new();
            static IDeserialize<CustomImArray<T>> IDeserializeProvider<CustomImArray<T>>.DeserializeInstance => Instance;
            public static ISerdeInfo SerdeInfo => s_typeInfo;

            private readonly IDeserialize<T> _proxy = TProvider.DeserializeInstance;
            private Deserialize() {}

            CustomImArray<T> IDeserialize<CustomImArray<T>>.Deserialize(IDeserializer deserializer)
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

                while (d.TryReadValue<T, IDeserialize<T>>(serdeInfo, _proxy, out T? next))
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

    internal static partial class CustomImArray2Proxy
    {
        private static readonly ISerdeInfo s_serdeInfo = new CollectionSerdeInfo(
            typeof(CustomImArray2<int>).ToString(),
            InfoKind.Enumerable);

        public sealed class Serialize<T, TProvider> : SerializeInstance<T, ISerialize<T>>, ISerializeProvider<CustomImArray2<T>>
            where TProvider : ISerializeProvider<T>
        {
            public static Serialize<T, TProvider> Instance { get; } = new();
            static ISerialize<CustomImArray2<T>> ISerializeProvider<CustomImArray2<T>>.SerializeInstance => Instance;
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_serdeInfo;


            private Serialize() : base(TProvider.SerializeInstance) { }
        }

        public class SerializeInstance<T, TProxy>(TProxy proxy) : ISerialize<CustomImArray2<T>>
            where TProxy : ISerialize<T>
        {
            void ISerialize<CustomImArray2<T>>.Serialize(CustomImArray2<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan(s_serdeInfo, value.Backing.AsSpan(), proxy, serializer);
        }

        public sealed class Deserialize<T, TProvider> : DeserializeInstance<T, IDeserialize<T>>, IDeserializeProvider<CustomImArray2<T>>
            where TProvider : IDeserializeProvider<T>
        {
            public static Deserialize<T, TProvider> Instance { get; } = new();
            static IDeserialize<CustomImArray2<T>> IDeserializeProvider<CustomImArray2<T>>.DeserializeInstance => Instance;
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => s_serdeInfo;

            private Deserialize() : base(TProvider.DeserializeInstance) { }
        }

        public class DeserializeInstance<T, TProxy>(TProxy proxy) : IDeserialize<CustomImArray2<T>>
            where TProxy : IDeserialize<T>
        {
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

                while (d.TryReadValue<T, TProxy>(typeInfo, proxy, out T? next))
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