//HintName: Test.Parent.ISerdeProvider.g.cs

namespace Test;

partial record Parent : Serde.ISerdeProvider<Test.Parent, Test.Parent._SerdeObj, Test.Parent>
{
    static Test.Parent._SerdeObj global::Serde.ISerdeProvider<Test.Parent, Test.Parent._SerdeObj, Test.Parent>.Instance { get; }
        = new Test.Parent._SerdeObj();
}
