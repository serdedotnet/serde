
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
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<StructWithIntField>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.X);
                _l_type.End(_l_info);
            }
            public static readonly StructWithIntFieldSerializeProxy Instance = new();
            private StructWithIntFieldSerializeProxy() { }

        }
    }
}
