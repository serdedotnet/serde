
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
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<CustomImArrayExplicitWrapOnMember>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayWrap.SerializeImpl<int, global::Serde.Int32Wrap>>(_l_serdeInfo, 0, value.A);
                type.End();
            }
        }
    }
}