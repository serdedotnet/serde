
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class GenericWrapperTests
{
    partial record struct CustomImArrayExplicitWrapOnMember : Serde.ISerializeProvider<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
    {
        static ISerialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember> ISerializeProvider<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.SerializeInstance
            => CustomImArrayExplicitWrapOnMemberSerializeProxy.Instance;

        sealed partial class CustomImArrayExplicitWrapOnMemberSerializeProxy :Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
        {
            void global::Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Serialize(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<CustomImArrayExplicitWrapOnMember>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteBoxedField<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayProxy.Serialize<int, global::Serde.I32Proxy>>(_l_info, 0, value.A);
                _l_type.End(_l_info);
            }
            public static readonly CustomImArrayExplicitWrapOnMemberSerializeProxy Instance = new();
            private CustomImArrayExplicitWrapOnMemberSerializeProxy() { }

        }
    }
}
