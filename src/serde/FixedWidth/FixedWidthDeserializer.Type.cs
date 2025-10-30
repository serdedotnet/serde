using System;
using System.Buffers;
using System.Globalization;

namespace Serde.FixedWidth
{
    internal sealed partial class FixedWidthDeserializer : ITypeDeserializer
    {
        private const NumberStyles Numeric = NumberStyles.Integer | NumberStyles.AllowThousands;

        int? ITypeDeserializer.SizeOpt => null;

        T ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize)
            => deserialize.Deserialize(this);

        string ITypeDeserializer.ReadString(ISerdeInfo info, int index) => _reader.ReadString(info, index);
        bool ITypeDeserializer.ReadBool(ISerdeInfo info, int index) => _reader.ReadBool(info, index);
        char ITypeDeserializer.ReadChar(ISerdeInfo info, int index) => _reader.ReadChar(info, index);
        DateTime ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index) => _reader.ReadDateTime(info, index);
        decimal ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index) => _reader.ReadNumber<decimal>(info, index, NumberStyles.Currency | NumberStyles.AllowLeadingSign);
        float ITypeDeserializer.ReadF32(ISerdeInfo info, int index) => _reader.ReadNumber<float>(info, index, NumberStyles.Float);
        double ITypeDeserializer.ReadF64(ISerdeInfo info, int index) => _reader.ReadNumber<double>(info, index, NumberStyles.Float);
        short ITypeDeserializer.ReadI16(ISerdeInfo info, int index) => _reader.ReadNumber<short>(info, index, Numeric);
        int ITypeDeserializer.ReadI32(ISerdeInfo info, int index) => _reader.ReadNumber<int>(info, index, Numeric);
        long ITypeDeserializer.ReadI64(ISerdeInfo info, int index) => _reader.ReadNumber<long>(info, index, Numeric);
        sbyte ITypeDeserializer.ReadI8(ISerdeInfo info, int index) => _reader.ReadNumber<sbyte>(info, index, Numeric);
        ushort ITypeDeserializer.ReadU16(ISerdeInfo info, int index) => _reader.ReadNumber<ushort>(info, index, Numeric);
        uint ITypeDeserializer.ReadU32(ISerdeInfo info, int index) => _reader.ReadNumber<uint>(info, index, Numeric);
        ulong ITypeDeserializer.ReadU64(ISerdeInfo info, int index) => _reader.ReadNumber<ulong>(info, index, Numeric);
        byte ITypeDeserializer.ReadU8(ISerdeInfo info, int index) => _reader.ReadNumber<byte>(info, index, Numeric);

        void ITypeDeserializer.SkipValue(ISerdeInfo info, int index) { }

        int ITypeDeserializer.TryReadIndex(ISerdeInfo info) => TryReadIndexWithName(info).Item1;

        (int, string? errorName) ITypeDeserializer.TryReadIndexWithName(ISerdeInfo info) => TryReadIndexWithName(info);

        private (int, string? errorName) TryReadIndexWithName(ISerdeInfo serdeInfo)
        {
            throw new NotImplementedException();
        }
        
        void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
        {
            throw new NotImplementedException();
        }
    }
}
