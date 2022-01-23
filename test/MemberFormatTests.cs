
using System.Threading.Tasks;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class MemberFormatTests
    {
        [Fact]
        public Task CamelCase()
        {
            var src = @"
using Serde;

[GenerateSerialize]
[SerdeTypeOptions(MemberFormat = MemberFormat.CamelCase)]
partial struct S
{
    public int One { get; set; }
    public int TwoWord { get; set; }
}";
            return VerifyGeneratedCode(src, "S.ISerialize", @"
#nullable enable
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 2);
        type.SerializeField(""one"", new Int32Wrap(this.One));
        type.SerializeField(""twoWord"", new Int32Wrap(this.TwoWord));
        type.End();
    }
}");
        }
    }
}