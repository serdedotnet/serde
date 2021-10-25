

using System.Threading.Tasks;
using Xunit;

namespace Serde.Test
{
    public class GeneratorWrapperTests
    {
        [Fact]
        public Task StringWrap()
        {
            var src = @"
using Serde;
[GenerateWrapper(nameof(_s))]
readonly partial struct StringWrap
{
    private readonly string _s;
    public StringWrap(string s)
    {
        _s = s;
    }
}";
            return GeneratorTests.VerifyGeneratedCode(src, "StringWrap", @"
using Serde;

partial struct StringWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
    {
        serializer.SerializeString(_s);
    }
}");

        }

        [Fact]
        public Task RecordStringWrap()
        {
            var src = @"
using Serde;
[GenerateWrapper(""Wrapped"")]
partial record struct StringWrap(string Wrapped);
";
            return GeneratorTests.VerifyGeneratedCode(src, "StringWrap", @"
using Serde;

partial record struct StringWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
    {
        serializer.SerializeString(Wrapped);
    }
}");

        }
    }
}