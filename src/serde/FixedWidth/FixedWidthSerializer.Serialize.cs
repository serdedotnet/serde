using System;
using System.Collections.Generic;
using System.Text;

namespace Serde.FixedWidth
{
    public sealed partial class FixedWidthSerializer : ISerializer
    {
        /// <inheritdoc/>
        ITypeSerializer ISerializer.WriteType(ISerdeInfo info)
        {
            if (info.Kind is not InfoKind.CustomType)
            {
                throw new ArgumentException("Invalid type for WriteType: " + info.Kind);
            }

            return this;
        }

        void ISerializer.WriteString(string s) => throw new NotImplementedException();
        void ISerializer.WriteBool(bool b) => throw new NotImplementedException();
        void ISerializer.WriteBytes(ReadOnlyMemory<byte> bytes) => throw new NotImplementedException();
        void ISerializer.WriteChar(char c) => throw new NotImplementedException();
        ITypeSerializer ISerializer.WriteCollection(ISerdeInfo info, int? count) => throw new NotImplementedException();
        void ISerializer.WriteDateTime(DateTime dt) => throw new NotImplementedException();
        void ISerializer.WriteDateTimeOffset(DateTimeOffset dt) => throw new NotImplementedException();
        void ISerializer.WriteDecimal(decimal d) => throw new NotImplementedException();
        void ISerializer.WriteF32(float f) => throw new NotImplementedException();
        void ISerializer.WriteF64(double d) => throw new NotImplementedException();
        void ISerializer.WriteI16(short i16) => throw new NotImplementedException();
        void ISerializer.WriteI32(int i32) => throw new NotImplementedException();
        void ISerializer.WriteI64(long i64) => throw new NotImplementedException();
        void ISerializer.WriteI8(sbyte b) => throw new NotImplementedException();
        void ISerializer.WriteNull() => throw new NotImplementedException();
        void ISerializer.WriteU16(ushort u16) => throw new NotImplementedException();
        void ISerializer.WriteU32(uint u32) => throw new NotImplementedException();
        void ISerializer.WriteU64(ulong u64) => throw new NotImplementedException();
        void ISerializer.WriteU8(byte b) => throw new NotImplementedException();
    }
}
