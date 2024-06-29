
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class MemberFormatTests
    {
        [Fact]
        public Task Default()
        {
            var src = @"
using Serde;

[GenerateSerialize]
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
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 2);
        type.SerializeField(""one"", new Int32Wrap(this.One));
        type.SerializeField(""twoWord"", new Int32Wrap(this.TwoWord));
        type.End();
    }
}");
        }

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
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 2);
        type.SerializeField(""one"", new Int32Wrap(this.One));
        type.SerializeField(""twoWord"", new Int32Wrap(this.TwoWord));
        type.End();
    }
}");
        }

        [Fact]
        public Task EnumValues()
        {
            var src = """
using Serde;
[GenerateSerialize, GenerateDeserialize]
partial struct S
{
    public ColorEnum E;
}
[GenerateSerialize, GenerateDeserialize]
[SerdeTypeOptions(MemberFormat = MemberFormat.None)]
partial struct S2
{
    public ColorEnum E;
}
[GenerateSerde]
enum ColorEnum
{
    Red,
    Green,
    Blue
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task KebabCase()
        {
            var src = @"
using Serde;

[GenerateSerde]
[SerdeTypeOptions(MemberFormat = MemberFormat.KebabCase)]
partial struct S
{
    public int One { get; set; }
    public int TwoWord { get; set; }
}";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task EnumFormat()
        {
            var src = """

using Serde;
[GenerateSerde]
[SerdeTypeOptions(MemberFormat = MemberFormat.None)]
public enum ColorEnum
{
    Red,
    Green,
    Blue
}
""";
            return VerifyMultiFile(src);
        }
   }
}