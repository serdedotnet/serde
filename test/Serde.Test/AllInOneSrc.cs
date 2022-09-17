
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

        public sbyte SByteField;
        public short ShortField;
        public int IntField;
        public long LongField;

        public string StringField = "StringValue";

        public string? NullStringField = null;

        public uint[] UIntArr = null!;
        public int[][] NestedArr = null!;

        public ImmutableArray<int> IntImm;

        public ColorEnum Color = ColorEnum.Blue;

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
                SByteField == other.SByteField &&
                ShortField == other.ShortField &&
                IntField == other.IntField &&
                LongField == other.LongField &&
                StringField == other.StringField &&
                NullStringField == other.NullStringField &&
                UIntArr.AsSpan().SequenceEqual(other.UIntArr.AsSpan()) &&
                NestedArr.AsSpan().SequenceEqual(other.NestedArr.AsSpan(),
                    new Comparer()) &&
                IntImm.AsSpan().SequenceEqual(other.IntImm.AsSpan()); // &&
                //Color == other.Color;
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

        internal static readonly AllInOne Sample = new AllInOne()
        {
            BoolField = true,
            CharField = '#',
            ByteField = byte.MaxValue,
            UShortField = ushort.MaxValue,
            UIntField = uint.MaxValue,
            ULongField = ulong.MaxValue,

            SByteField = sbyte.MaxValue,
            ShortField = short.MaxValue,
            IntField = int.MaxValue,
            LongField = long.MaxValue,

            StringField = "StringValue",

            UIntArr = new uint[] { 1, 2, 3 },
            NestedArr = new[] { new[] { 1 }, new[] { 2 } },

            IntImm = ImmutableArray.Create<int>(1, 2)
        };
    }
}