
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record EmptyRecord
    {
        private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "EmptyRecord",
        typeof(Serde.Test.SerdeInfoTests.EmptyRecord).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {

        }
        );
    }
}