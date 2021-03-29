
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Serde.Test
{
    public class GeneratorTests
    {
        [Fact]
        public Task Rgb()
        {
            var src = @"
using Serde;

[GenerateSerde]
partial class Rgb
{
    public byte Red;
    public byte Green;
    public byte Blue;
}";
            var verifier = new CSharpSourceGeneratorTest<SerdeGenerator, XUnitVerifier>()
            {
                TestCode = src
            };
            verifier.ReferenceAssemblies = verifier.ReferenceAssemblies.AddAssemblies(
                ImmutableArray.Create(typeof(Serde.GenerateSerdeAttribute).Assembly.Location));
            verifier.TestState.GeneratedSources.Add(("SerdeGenerator\\Serde.SerdeGenerator\\Rgb.Serde.cs", SourceText.From(@"
partial class Rgb : Serde.ISerialize
{
    public void Serialize<TSerializer, TSerializeStruct>(TSerializer serializer)
        where TSerializer : Serde.ISerializer<TSerializeStruct>
        where TSerializeStruct : Serde.ISerializeStruct
    {
        var type = serializer.SerializeStruct(""Rgb"", 3);
        type.SerializeField(""Red"", Red);
        type.SerializeField(""Green"", Green);
        type.SerializeField(""Blue"", Blue);
        type.End();
    }
}", Encoding.UTF8)));
            return verifier.RunAsync();
        }
    }
}