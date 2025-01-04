
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial record StructWithIntField : Serde.ISerializeProvider<Serde.Test.XmlTests.StructWithIntField>
    {
        static ISerialize<Serde.Test.XmlTests.StructWithIntField> ISerializeProvider<Serde.Test.XmlTests.StructWithIntField>.SerializeInstance
            => StructWithIntFieldSerializeProxy.Instance;

        sealed partial class StructWithIntFieldSerializeProxy :Serde.ISerialize<Serde.Test.XmlTests.StructWithIntField>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.StructWithIntField>.Serialize(Serde.Test.XmlTests.StructWithIntField value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<StructWithIntField>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,0,value.X);
                type.End();
            }
            public static readonly StructWithIntFieldSerializeProxy Instance = new();
            private StructWithIntFieldSerializeProxy() { }

        }
    }
}
