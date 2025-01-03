
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class TypeWithArrayField : Serde.ISerializeProvider<Serde.Test.XmlTests.TypeWithArrayField>
        {
            static ISerialize<Serde.Test.XmlTests.TypeWithArrayField> ISerializeProvider<Serde.Test.XmlTests.TypeWithArrayField>.SerializeInstance => TypeWithArrayFieldSerializeProxy.Instance;

            sealed class TypeWithArrayFieldSerializeProxy : Serde.ISerialize<Serde.Test.XmlTests.TypeWithArrayField>
            {
                void ISerialize<Serde.Test.XmlTests.TypeWithArrayField>.Serialize(Serde.Test.XmlTests.TypeWithArrayField value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<TypeWithArrayField>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayProxy.Serialize<Serde.Test.XmlTests.StructWithIntField, Serde.Test.XmlTests.StructWithIntField>>(_l_serdeInfo, 0, value.ArrayField);
                    type.End();
                }

                public static readonly TypeWithArrayFieldSerializeProxy Instance = new();
                private TypeWithArrayFieldSerializeProxy()
                {
                }
            }
        }
    }
}