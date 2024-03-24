
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class CustomImplTests
    {
        partial record RgbWithFieldMap : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("RgbWithFieldMap", 3);
                type.SerializeField("red"u8, new Int32Wrap(this.Red));
                type.SerializeField("green"u8, new Int32Wrap(this.Green));
                type.SerializeField("blue"u8, new Int32Wrap(this.Blue));
                type.End();
            }
        }
    }
}