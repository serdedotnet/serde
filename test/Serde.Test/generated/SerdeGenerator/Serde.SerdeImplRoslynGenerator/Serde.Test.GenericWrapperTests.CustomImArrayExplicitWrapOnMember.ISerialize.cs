
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomImArrayExplicitWrapOnMember : Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
        {
            void ISerialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Serialize(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember value, ISerializer serializer)
            {
                var _l_typeInfo = CustomImArrayExplicitWrapOnMemberSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayWrap.SerializeImpl<int, Int32Wrap>>(_l_typeInfo, 0, this.A);
                type.End();
            }
        }
    }
}