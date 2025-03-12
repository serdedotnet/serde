
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class GenericWrapperTests
{
    partial record struct CustomArrayWrapExplicitOnType : Serde.ISerializeProvider<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
    {
        static ISerialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType> ISerializeProvider<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.SerializeInstance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
        {
            void global::Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.Serialize(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<CustomArrayWrapExplicitOnType>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteBoxedValue<Serde.Test.GenericWrapperTests.CustomImArray2<int>, Serde.Test.GenericWrapperTests.CustomImArray2Proxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 0, value.A);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
