
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial struct BoolStruct : Serde.ISerializeProvider<Serde.Test.XmlTests.BoolStruct>
    {
        static ISerialize<Serde.Test.XmlTests.BoolStruct> ISerializeProvider<Serde.Test.XmlTests.BoolStruct>.SerializeInstance
            => BoolStructSerializeProxy.Instance;

        sealed partial class BoolStructSerializeProxy :Serde.ISerialize<Serde.Test.XmlTests.BoolStruct>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.BoolStruct>.Serialize(Serde.Test.XmlTests.BoolStruct value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<BoolStruct>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<bool,global::Serde.BoolProxy>(_l_serdeInfo,0,value.BoolField);
                type.End();
            }
            public static readonly BoolStructSerializeProxy Instance = new();
            private BoolStructSerializeProxy() { }

        }
    }
}
