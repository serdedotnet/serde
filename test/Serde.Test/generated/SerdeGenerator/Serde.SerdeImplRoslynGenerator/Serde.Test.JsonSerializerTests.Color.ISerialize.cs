
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial struct Color : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("Color", 3);
                type.SerializeField("red"u8, new Int32Wrap(this.Red));
                type.SerializeField("green"u8, new Int32Wrap(this.Green));
                type.SerializeField("blue"u8, new Int32Wrap(this.Blue));
                type.End();
            }
        }
    }
}