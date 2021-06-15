
// Contains implementations of data interfaces for core types

using System.Collections.Generic;

namespace Serde
{
    public readonly struct BoolWrap : ISerialize
    {
        private readonly bool _b;
        public BoolWrap(bool b) { _b = b; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_b);
        }
    }

    public readonly struct CharWrap : ISerialize
    {
        private readonly char _c;
        public CharWrap(char c) { _c = c; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_c);
        }
    }

    public readonly struct ByteWrap : ISerialize
    {
        private readonly byte _b;
        public ByteWrap(byte b) { _b = b;}

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_b);
        }
    }

    public readonly struct UInt16Wrap : ISerialize
    {
        private readonly ushort _i;
        public UInt16Wrap(ushort i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct UInt32Wrap : ISerialize
    {
        private readonly uint _i;
        public UInt32Wrap(uint i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct UInt64Wrap : ISerialize
    {
        private readonly ulong _i;
        public UInt64Wrap(ulong i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct SByteWrap : ISerialize
    {
        private readonly sbyte _i;
        public SByteWrap(sbyte i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int16Wrap : ISerialize
    {
        private readonly short _i;
        public Int16Wrap(short i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int32Wrap : ISerialize
    {
        private readonly int _i;
        public Int32Wrap(int i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int64Wrap : ISerialize
    {
        private readonly long _i;
        public Int64Wrap(long i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct StringWrap : ISerialize
    {
        private readonly string _s;
        public StringWrap(string s) { _s = s; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_s);
        }
    }

    public readonly struct EnumerableWrap<T, TEnum> : ISerialize
        where T : ISerialize
        where TEnum : IEnumerable<T>
    {
        private readonly int? _count;
        private readonly TEnum _enumerable;
        public EnumerableWrap(int? count, TEnum enumerable)
        {
            _count = count;
            _enumerable = enumerable;
        }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            var sEnumerable = serializer.SerializeEnumerable(_count);
            foreach (var elem in _enumerable)
            {
                sEnumerable.SerializeElement(elem);
            }
            sEnumerable.End();
        }
    }
}