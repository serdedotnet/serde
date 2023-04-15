
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomImArrayExplicitWrapOnMember : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("CustomImArrayExplicitWrapOnMember", 1);
                type.SerializeField("a", new Serde.Test.GenericWrapperTests.CustomImArrayWrap.SerializeImpl<int, Int32Wrap>(this.A));
                type.End();
            }
        }
    }
}