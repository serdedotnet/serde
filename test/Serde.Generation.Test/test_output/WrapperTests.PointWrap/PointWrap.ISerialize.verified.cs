﻿//HintName: PointWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct PointWrap : Serde.ISerializeProvider<Point>
{
    static ISerialize<Point> ISerializeProvider<Point>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<Point>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => PointWrap.s_serdeInfo;

        void global::Serde.ISerialize<Point>.Serialize(Point value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.X);
            _l_type.WriteI32(_l_info, 1, value.Y);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
