

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
partial struct StringWrap
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
    }
}