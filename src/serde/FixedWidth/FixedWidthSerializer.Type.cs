using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Serde.FixedWidth
{
    public sealed partial class FixedWidthSerializer : ITypeSerializer
    {
        void ITypeSerializer.WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            => serialize.Serialize(value, this);

        void ITypeSerializer.End(ISerdeInfo info) => writer.WriteLine();
        void ITypeSerializer.WriteBool(ISerdeInfo typeInfo, int index, bool b) => writer.WriteObject(typeInfo, index, b);
        void ITypeSerializer.WriteChar(ISerdeInfo typeInfo, int index, char c) => writer.WriteObject(typeInfo, index, c);
        void ITypeSerializer.WriteU8(ISerdeInfo typeInfo, int index, byte b) => writer.WriteObject(typeInfo, index, b);
        void ITypeSerializer.WriteU16(ISerdeInfo typeInfo, int index, ushort u16) => writer.WriteObject(typeInfo, index, u16);
        void ITypeSerializer.WriteU32(ISerdeInfo typeInfo, int index, uint u32) => writer.WriteObject(typeInfo, index, u32);
        void ITypeSerializer.WriteU64(ISerdeInfo typeInfo, int index, ulong u64) => writer.WriteObject(typeInfo, index, u64);
        void ITypeSerializer.WriteI8(ISerdeInfo typeInfo, int index, sbyte b) => writer.WriteObject(typeInfo, index, b);
        void ITypeSerializer.WriteI16(ISerdeInfo typeInfo, int index, short i16) => writer.WriteObject(typeInfo, index, i16);
        void ITypeSerializer.WriteI32(ISerdeInfo typeInfo, int index, int i32) => writer.WriteObject(typeInfo, index, i32);
        void ITypeSerializer.WriteI64(ISerdeInfo typeInfo, int index, long i64) => writer.WriteObject(typeInfo, index, i64);
        void ITypeSerializer.WriteF32(ISerdeInfo typeInfo, int index, float f) => writer.WriteObject(typeInfo, index, f);
        void ITypeSerializer.WriteF64(ISerdeInfo typeInfo, int index, double d) => writer.WriteObject(typeInfo, index, d);
        void ITypeSerializer.WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) => writer.WriteObject(typeInfo, index, d);
        void ITypeSerializer.WriteString(ISerdeInfo typeInfo, int index, string s) => writer.WriteObject(typeInfo, index, s);
        void ITypeSerializer.WriteNull(ISerdeInfo typeInfo, int index) => writer.WriteObject(typeInfo, index, string.Empty);
        void ITypeSerializer.WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt) => writer.WriteObject(typeInfo, index, dt);
        void ITypeSerializer.WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt) => writer.WriteObject(typeInfo, index, dt);
        void ITypeSerializer.WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes) => writer.WriteObject(typeInfo, index, bytes);
    }
}
