
using System.Collections.Immutable;

namespace Serde.Test
{
    public partial class AllInOne
    {
        public bool BoolField = true;
        public char CharField = '#';
        public byte ByteField = byte.MaxValue;
        public ushort UShortField = ushort.MaxValue;
        public uint UIntField = uint.MaxValue;
        public ulong ULongField = ulong.MaxValue;

        public sbyte SByteField = sbyte.MaxValue;
        public short ShortField = short.MaxValue;
        public int IntField = int.MaxValue;
        public long LongField = long.MaxValue;

        public string StringField = "StringValue";

        public int[] IntArr = new[] { 1, 2, 3 };
        public int[][] NestedArr = new[] { new[] { 1 }, new[] { 2 } };

        public ImmutableArray<int> IntImm = ImmutableArray.Create<int>(1, 2);
    }
}