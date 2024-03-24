
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class CustomImplTests
    {
        partial record RgbWithFieldMap : Serde.ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap>
        {
            void ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap>.Serialize(Serde.Test.CustomImplTests.RgbWithFieldMap value, ISerializer serializer)
            {
                var type = serializer.SerializeType("RgbWithFieldMap", 3);
                type.SerializeField<int, Int32Wrap>("red", value.Red);
                type.SerializeField<int, Int32Wrap>("green", value.Green);
                type.SerializeField<int, Int32Wrap>("blue", value.Blue);
                type.End();
            }
        }
    }
}