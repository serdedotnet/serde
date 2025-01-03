
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial struct Color : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.Color>
        {
            static ISerialize<Serde.Test.JsonSerializerTests.Color> ISerializeProvider<Serde.Test.JsonSerializerTests.Color>.SerializeInstance => ColorSerializeProxy.Instance;

            sealed class ColorSerializeProxy : Serde.ISerialize<Serde.Test.JsonSerializerTests.Color>
            {
                void ISerialize<Serde.Test.JsonSerializerTests.Color>.Serialize(Serde.Test.JsonSerializerTests.Color value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Color>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 0, value.Red);
                    type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 1, value.Green);
                    type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 2, value.Blue);
                    type.End();
                }

                public static readonly ColorSerializeProxy Instance = new();
                private ColorSerializeProxy()
                {
                }
            }
        }
    }
}