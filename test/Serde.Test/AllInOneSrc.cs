
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Serde.Test
{

    public partial class GenericType<T>
    {
        public int Field;
    }
    [GenerateSerialize]
    [GenerateDeserialize]
    public sealed partial record AllInOne
    {
        public bool BoolField;
        public char CharField;
        public byte ByteField;
        public ushort UShortField;
        public uint UIntField;
        public ulong ULongField;
        public UInt128 UInt128Field;

        public sbyte SByteField;
        public short ShortField;
        public int IntField;
        public long LongField;
        public Int128 Int128Field;

        public string StringField = "StringValue";
        public DateTimeOffset DateTimeOffsetField;
        public DateTime DateTimeField;
        public DateOnly DateOnlyField;
        public TimeOnly TimeOnlyField;
        public Guid GuidField;

        public required string EscapedStringField;

        public string? NullStringField = null;

        public uint[] UIntArr = null!;
        public int[][] NestedArr = null!;
        public byte[] ByteArr = null!;

        public ImmutableArray<int> IntImm;

        public ColorEnum Color;

        [GenerateSerde]
        public enum ColorEnum
        {
            Red = 1,
            Blue = 3,
            Green = 5
        }

        // implement Equals to do deep equals for the collections
        public bool Equals(AllInOne? other)
        {
            return other is not null &&
                BoolField == other.BoolField &&
                CharField == other.CharField &&
                ByteField == other.ByteField &&
                UShortField == other.UShortField &&
                UIntField == other.UIntField &&
                ULongField == other.ULongField &&
                UInt128Field == other.UInt128Field &&
                SByteField == other.SByteField &&
                ShortField == other.ShortField &&
                IntField == other.IntField &&
                LongField == other.LongField &&
                Int128Field == other.Int128Field &&
                StringField == other.StringField &&
                DateTimeOffsetField == other.DateTimeOffsetField &&
                DateTimeField == other.DateTimeField &&
                DateOnlyField == other.DateOnlyField &&
                TimeOnlyField == other.TimeOnlyField &&
                EscapedStringField == other.EscapedStringField &&
                GuidField.Equals(other.GuidField) &&
                NullStringField == other.NullStringField &&
                UIntArr.AsSpan().SequenceEqual(other.UIntArr.AsSpan()) &&
                NestedArr.AsSpan().SequenceEqual(other.NestedArr.AsSpan(),
                    new Comparer()) &&
                ByteArr.AsSpan().SequenceEqual(other.ByteArr.AsSpan()) &&
                IntImm.AsSpan().SequenceEqual(other.IntImm.AsSpan()) &&
                Color == other.Color;
        }
        private sealed class Comparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[]? x, int[]? y)
            {
                return x?.AsSpan().SequenceEqual(y.AsSpan()) ?? y == null;
            }

            public int GetHashCode([DisallowNull] int[] obj)
            {
                throw new NotImplementedException();
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static readonly AllInOne Sample = new AllInOne()
        {
            BoolField = true,
            CharField = '#',
            ByteField = byte.MaxValue,
            UShortField = ushort.MaxValue,
            UIntField = uint.MaxValue,
            ULongField = ulong.MaxValue,
            UInt128Field = UInt128.MaxValue,

            SByteField = sbyte.MaxValue,
            ShortField = short.MaxValue,
            IntField = int.MaxValue,
            LongField = long.MaxValue,
            Int128Field = Int128.MaxValue,
            StringField = "StringValue",
            DateTimeOffsetField = new DateTimeOffset(2040, 1, 1, 1, 1, 1, TimeSpan.FromHours(-7)),
            DateTimeField = new DateTime(2040, 1, 1, 1, 1, 1, DateTimeKind.Utc),
            DateOnlyField = new DateOnly(2040, 6, 15),
            TimeOnlyField = new TimeOnly(14, 30, 45),
            GuidField = new Guid(new byte[] {
                0x01, 0x02, 0x03, 0x04,
                0x05, 0x06, 0x07, 0x08,
                0x09, 0x0A, 0x0B, 0x0C,
                0x0D, 0x0E, 0x0F, 0x10,
            }),

            EscapedStringField = "+0 11 222 333 44",

            UIntArr = new uint[] { 1, 2, 3 },
            NestedArr = new[] { new[] { 1 }, new[] { 2 } },
            ByteArr = new byte[] { 1, 2, 3 },

            IntImm = ImmutableArray.Create<int>(1, 2),

            Color = ColorEnum.Blue
        };

        public const string SampleSerialized = """
{
  "boolField": true,
  "charField": "#",
  "byteField": 255,
  "uShortField": 65535,
  "uIntField": 4294967295,
  "uLongField": 18446744073709551615,
  "uInt128Field": 340282366920938463463374607431768211455,
  "sByteField": 127,
  "shortField": 32767,
  "intField": 2147483647,
  "longField": 9223372036854775807,
  "int128Field": 170141183460469231731687303715884105727,
  "stringField": "StringValue",
  "dateTimeOffsetField": "2040-01-01T01:01:01-07:00",
  "dateTimeField": "2040-01-01T01:01:01Z",
  "dateOnlyField": "2040-06-15",
  "timeOnlyField": "14:30:45",
  "guidField": "04030201-0605-0807-090a-0b0c0d0e0f10",
  "escapedStringField": "\u002B0 11 222 333 44",
  "uIntArr": [
    1,
    2,
    3
  ],
  "nestedArr": [
    [
      1
    ],
    [
      2
    ]
  ],
  "byteArr": "AQID",
  "intImm": [
    1,
    2
  ],
  "color": "blue"
}
""";

    }
}