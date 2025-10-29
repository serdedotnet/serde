using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Serde.FixedWidth
{
    internal sealed partial class FixedWidthDeserializer : ITypeDeserializer
    {
        int? ITypeDeserializer.SizeOpt => null;

        bool ITypeDeserializer.ReadBool(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
        {
            throw new NotImplementedException();
        }

        char ITypeDeserializer.ReadChar(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        DateTime ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        decimal ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        float ITypeDeserializer.ReadF32(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        double ITypeDeserializer.ReadF64(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        short ITypeDeserializer.ReadI16(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        int ITypeDeserializer.ReadI32(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        long ITypeDeserializer.ReadI64(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        sbyte ITypeDeserializer.ReadI8(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        string ITypeDeserializer.ReadString(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        ushort ITypeDeserializer.ReadU16(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        uint ITypeDeserializer.ReadU32(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        ulong ITypeDeserializer.ReadU64(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        byte ITypeDeserializer.ReadU8(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        T ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize)
        {
            throw new NotImplementedException();
        }

        void ITypeDeserializer.SkipValue(ISerdeInfo info, int index)
        {
            throw new NotImplementedException();
        }

        int ITypeDeserializer.TryReadIndex(ISerdeInfo info)
        {
            throw new NotImplementedException();
        }

        (int, string? errorName) ITypeDeserializer.TryReadIndexWithName(ISerdeInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
