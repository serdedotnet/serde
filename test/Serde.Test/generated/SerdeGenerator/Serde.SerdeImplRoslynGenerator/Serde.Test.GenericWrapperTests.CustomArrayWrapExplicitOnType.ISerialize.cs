
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomArrayWrapExplicitOnType : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("CustomArrayWrapExplicitOnType", 1);
                type.SerializeField("a", new Serde.Test.GenericWrapperTests.CustomImArray2Wrap.SerializeImpl<int, Int32Wrap>(this.A));
                type.End();
            }
        }
    }
}