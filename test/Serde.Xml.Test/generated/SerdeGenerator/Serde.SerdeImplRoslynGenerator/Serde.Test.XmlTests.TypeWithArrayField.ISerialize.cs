
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial class TypeWithArrayField : Serde.ISerializeProvider<Serde.Test.XmlTests.TypeWithArrayField>
    {
        static ISerialize<Serde.Test.XmlTests.TypeWithArrayField> ISerializeProvider<Serde.Test.XmlTests.TypeWithArrayField>.SerializeInstance
            => TypeWithArrayFieldSerializeProxy.Instance;

        sealed partial class TypeWithArrayFieldSerializeProxy :Serde.ISerialize<Serde.Test.XmlTests.TypeWithArrayField>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.TypeWithArrayField>.Serialize(Serde.Test.XmlTests.TypeWithArrayField value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<TypeWithArrayField>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteField<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayProxy.Serialize<Serde.Test.XmlTests.StructWithIntField, Serde.Test.XmlTests.StructWithIntField>>(_l_info, 0, value.ArrayField);
                _l_type.End(_l_info);
            }
            public static readonly TypeWithArrayFieldSerializeProxy Instance = new();
            private TypeWithArrayFieldSerializeProxy() { }

        }
    }
}
