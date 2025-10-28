using System;
using System.Collections.Generic;
using System.Text;

namespace Serde.FixedWidth
{
    public sealed partial class FixedWidthSerializer
    {
        /// <inheritdoc/>
        public void WriteString(string s) => writer.WriteLine(s);

        /// <inheritdoc/>
        public void WriteBool(bool b) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteBytes(ReadOnlyMemory<byte> bytes) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteChar(char c) => throw new NotImplementedException();

        /// <inheritdoc/>
        public ITypeSerializer WriteCollection(ISerdeInfo info, int? count) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteDateTime(DateTime dt) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteDateTimeOffset(DateTimeOffset dt) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteDecimal(decimal d) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteF32(float f) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteF64(double d) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteI16(short i16) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteI32(int i32) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteI64(long i64) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteI8(sbyte b) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteNull() => throw new NotImplementedException();

        /// <inheritdoc/>
        public ITypeSerializer WriteType(ISerdeInfo info) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteU16(ushort u16) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteU32(uint u32) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteU64(ulong u64) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void WriteU8(byte b) => throw new NotImplementedException();
    }
}
