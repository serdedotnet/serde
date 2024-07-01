
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class MapTest1 : Serde.ISerialize<Serde.Test.XmlTests.MapTest1>
        {
            void ISerialize<Serde.Test.XmlTests.MapTest1>.Serialize(Serde.Test.XmlTests.MapTest1 value, ISerializer serializer)
            {
                var _l_typeInfo = MapTest1SerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<System.Collections.Generic.Dictionary<string, int>, Serde.DictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>>(_l_typeInfo, 0, this.MapField);
                type.End();
            }
        }
    }
}