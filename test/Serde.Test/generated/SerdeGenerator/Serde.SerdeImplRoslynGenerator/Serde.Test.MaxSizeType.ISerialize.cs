
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial struct MaxSizeType : Serde.ISerializeProvider<Serde.Test.MaxSizeType>
{
    static ISerialize<Serde.Test.MaxSizeType> ISerializeProvider<Serde.Test.MaxSizeType>.SerializeInstance
        => MaxSizeTypeSerializeProxy.Instance;

    sealed partial class MaxSizeTypeSerializeProxy :Serde.ISerialize<Serde.Test.MaxSizeType>
    {
        void global::Serde.ISerialize<Serde.Test.MaxSizeType>.Serialize(Serde.Test.MaxSizeType value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<MaxSizeType>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteU8(_l_info, 0, value.Field1);
            _l_type.WriteU8(_l_info, 1, value.Field2);
            _l_type.WriteU8(_l_info, 2, value.Field3);
            _l_type.WriteU8(_l_info, 3, value.Field4);
            _l_type.WriteU8(_l_info, 4, value.Field5);
            _l_type.WriteU8(_l_info, 5, value.Field6);
            _l_type.WriteU8(_l_info, 6, value.Field7);
            _l_type.WriteU8(_l_info, 7, value.Field8);
            _l_type.WriteU8(_l_info, 8, value.Field9);
            _l_type.WriteU8(_l_info, 9, value.Field10);
            _l_type.WriteU8(_l_info, 10, value.Field11);
            _l_type.WriteU8(_l_info, 11, value.Field12);
            _l_type.WriteU8(_l_info, 12, value.Field13);
            _l_type.WriteU8(_l_info, 13, value.Field14);
            _l_type.WriteU8(_l_info, 14, value.Field15);
            _l_type.WriteU8(_l_info, 15, value.Field16);
            _l_type.WriteU8(_l_info, 16, value.Field17);
            _l_type.WriteU8(_l_info, 17, value.Field18);
            _l_type.WriteU8(_l_info, 18, value.Field19);
            _l_type.WriteU8(_l_info, 19, value.Field20);
            _l_type.WriteU8(_l_info, 20, value.Field21);
            _l_type.WriteU8(_l_info, 21, value.Field22);
            _l_type.WriteU8(_l_info, 22, value.Field23);
            _l_type.WriteU8(_l_info, 23, value.Field24);
            _l_type.WriteU8(_l_info, 24, value.Field25);
            _l_type.WriteU8(_l_info, 25, value.Field26);
            _l_type.WriteU8(_l_info, 26, value.Field27);
            _l_type.WriteU8(_l_info, 27, value.Field28);
            _l_type.WriteU8(_l_info, 28, value.Field29);
            _l_type.WriteU8(_l_info, 29, value.Field30);
            _l_type.WriteU8(_l_info, 30, value.Field31);
            _l_type.WriteU8(_l_info, 31, value.Field32);
            _l_type.WriteU8(_l_info, 32, value.Field33);
            _l_type.WriteU8(_l_info, 33, value.Field34);
            _l_type.WriteU8(_l_info, 34, value.Field35);
            _l_type.WriteU8(_l_info, 35, value.Field36);
            _l_type.WriteU8(_l_info, 36, value.Field37);
            _l_type.WriteU8(_l_info, 37, value.Field38);
            _l_type.WriteU8(_l_info, 38, value.Field39);
            _l_type.WriteU8(_l_info, 39, value.Field40);
            _l_type.WriteU8(_l_info, 40, value.Field41);
            _l_type.WriteU8(_l_info, 41, value.Field42);
            _l_type.WriteU8(_l_info, 42, value.Field43);
            _l_type.WriteU8(_l_info, 43, value.Field44);
            _l_type.WriteU8(_l_info, 44, value.Field45);
            _l_type.WriteU8(_l_info, 45, value.Field46);
            _l_type.WriteU8(_l_info, 46, value.Field47);
            _l_type.WriteU8(_l_info, 47, value.Field48);
            _l_type.WriteU8(_l_info, 48, value.Field49);
            _l_type.WriteU8(_l_info, 49, value.Field50);
            _l_type.WriteU8(_l_info, 50, value.Field51);
            _l_type.WriteU8(_l_info, 51, value.Field52);
            _l_type.WriteU8(_l_info, 52, value.Field53);
            _l_type.WriteU8(_l_info, 53, value.Field54);
            _l_type.WriteU8(_l_info, 54, value.Field55);
            _l_type.WriteU8(_l_info, 55, value.Field56);
            _l_type.WriteU8(_l_info, 56, value.Field57);
            _l_type.WriteU8(_l_info, 57, value.Field58);
            _l_type.WriteU8(_l_info, 58, value.Field59);
            _l_type.WriteU8(_l_info, 59, value.Field60);
            _l_type.WriteU8(_l_info, 60, value.Field61);
            _l_type.WriteU8(_l_info, 61, value.Field62);
            _l_type.WriteU8(_l_info, 62, value.Field63);
            _l_type.WriteU8(_l_info, 63, value.Field64);
            _l_type.End(_l_info);
        }
        public static readonly MaxSizeTypeSerializeProxy Instance = new();
        private MaxSizeTypeSerializeProxy() { }

    }
}
