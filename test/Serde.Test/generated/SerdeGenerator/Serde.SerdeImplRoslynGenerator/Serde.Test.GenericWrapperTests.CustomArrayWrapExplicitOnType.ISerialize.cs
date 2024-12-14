
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomArrayWrapExplicitOnType : Serde.ISerializeProvider<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
        {
            static ISerialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType> ISerializeProvider<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.SerializeInstance => CustomArrayWrapExplicitOnTypeSerializeProxy.Instance;

            sealed class CustomArrayWrapExplicitOnTypeSerializeProxy : Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
            {
                void ISerialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.Serialize(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<CustomArrayWrapExplicitOnType>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<Serde.Test.GenericWrapperTests.CustomImArray2<int>, Serde.Test.GenericWrapperTests.CustomImArray2Proxy.Serialize<int, global::Serde.Int32Proxy>>(_l_serdeInfo, 0, value.A);
                    type.End();
                }

                public static readonly CustomArrayWrapExplicitOnTypeSerializeProxy Instance = new();
                private CustomArrayWrapExplicitOnTypeSerializeProxy()
                {
                }
            }
        }
    }
}