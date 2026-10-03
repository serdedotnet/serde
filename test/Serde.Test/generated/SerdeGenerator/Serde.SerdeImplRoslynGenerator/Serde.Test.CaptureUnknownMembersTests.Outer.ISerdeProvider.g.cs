
namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial record Outer : Serde.ISerdeProvider<Serde.Test.CaptureUnknownMembersTests.Outer, Serde.Test.CaptureUnknownMembersTests.Outer._SerdeObj, Serde.Test.CaptureUnknownMembersTests.Outer>
    {
        static Serde.Test.CaptureUnknownMembersTests.Outer._SerdeObj global::Serde.ISerdeProvider<Serde.Test.CaptureUnknownMembersTests.Outer, Serde.Test.CaptureUnknownMembersTests.Outer._SerdeObj, Serde.Test.CaptureUnknownMembersTests.Outer>.Instance { get; }
            = new Serde.Test.CaptureUnknownMembersTests.Outer._SerdeObj();
    }
}
