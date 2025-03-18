
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial struct Color : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.Color>
    {
        static ISerialize<Serde.Test.JsonSerializerTests.Color> ISerializeProvider<Serde.Test.JsonSerializerTests.Color>.SerializeInstance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.JsonSerializerTests.Color>
        {
            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.Color>.Serialize(Serde.Test.JsonSerializerTests.Color value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Color>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.Red);
                _l_type.WriteI32(_l_info, 1, value.Green);
                _l_type.WriteI32(_l_info, 2, value.Blue);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
