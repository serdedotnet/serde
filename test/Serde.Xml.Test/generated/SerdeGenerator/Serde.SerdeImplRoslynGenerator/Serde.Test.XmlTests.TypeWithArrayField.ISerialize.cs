
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial class TypeWithArrayField : Serde.ISerializeProvider<Serde.Test.XmlTests.TypeWithArrayField>
    {
        static ISerialize<Serde.Test.XmlTests.TypeWithArrayField> ISerializeProvider<Serde.Test.XmlTests.TypeWithArrayField>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.XmlTests.TypeWithArrayField>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.XmlTests.TypeWithArrayField.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.XmlTests.TypeWithArrayField>.Serialize(Serde.Test.XmlTests.TypeWithArrayField value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValue<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayProxy.Ser<Serde.Test.XmlTests.StructWithIntField, Serde.Test.XmlTests.StructWithIntField>>(_l_info, 0, value.ArrayField);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
