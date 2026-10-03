//HintName: Shape.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial record Shape
{
    sealed partial class _DeObj : Serde.IDeserialize<Shape>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Shape.s_serdeInfo;

        Shape IDeserialize<Shape>.Deserialize(IDeserializer deserializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var de = deserializer.ReadType(_l_serdeInfo);
            var (index, errorName) = de.TryReadIndexWithName(_l_serdeInfo);
            if (index == ITypeDeserializer.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
            }
            Shape _l_result = index switch {
                0 => de.ReadValue<Shape.Install, _m_InstallProxy>(_l_serdeInfo, 0),

                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
            index = de.TryReadIndex(_l_serdeInfo);
            if (index != ITypeDeserializer.EndOfType)
            {
                throw Serde.DeserializeException.ExpectedEndOfType(index);
            }
            de.End(_l_serdeInfo);
            return _l_result;
        }
    }
}
