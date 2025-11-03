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

        void ISerializer.WriteString(string s) => writer.WriteRaw(s);
        void ISerializer.WriteBool(bool b) => writer.WriteRaw(b);
        void ISerializer.WriteBytes(ReadOnlyMemory<byte> bytes) => throw new NotImplementedException();
        void ISerializer.WriteChar(char c) => writer.WriteRaw(c);
        ITypeSerializer ISerializer.WriteCollection(ISerdeInfo info, int? count) => throw new NotImplementedException();
        void ISerializer.WriteDateTime(DateTime dt) => writer.WriteRaw(dt);
        void ISerializer.WriteDateTimeOffset(DateTimeOffset dto) => writer.WriteRaw(dto);
        void ISerializer.WriteDecimal(decimal d) => writer.WriteRaw(d);
        void ISerializer.WriteF32(float f) => writer.WriteRaw(f);
        void ISerializer.WriteF64(double d) => writer.WriteRaw(d);
        void ISerializer.WriteI16(short i16) => writer.WriteRaw(i16);
        void ISerializer.WriteI32(int i32) => writer.WriteRaw(i32);
        void ISerializer.WriteI64(long i64) => writer.WriteRaw(i64);
        void ISerializer.WriteI8(sbyte b) => writer.WriteRaw(b);
        void ISerializer.WriteNull() { }
        void ISerializer.WriteU16(ushort u16) => writer.WriteRaw(u16);
        void ISerializer.WriteU32(uint u32) => writer.WriteRaw(u32);
        void ISerializer.WriteU64(ulong u64) => writer.WriteRaw(u64);
        void ISerializer.WriteU8(byte b) => writer.WriteRaw(b);
    }
}
