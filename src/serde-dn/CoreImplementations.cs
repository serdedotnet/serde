
// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;

namespace Serde
{
    public readonly struct BoolWrap : ISerialize
    {
        public static readonly Func<bool, BoolWrap> Ctor = b => new BoolWrap(b);

        private readonly bool _b;
        public BoolWrap(bool b) { _b = b; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_b);
        }
    }

    public readonly struct CharWrap : ISerialize
    {
        public static readonly Func<char, CharWrap> Ctor = c => new CharWrap(c);

        private readonly char _c;
        public CharWrap(char c) { _c = c; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_c);
        }
    }

    public readonly struct ByteWrap : ISerialize
    {
        public static readonly Func<byte, ByteWrap> Ctor = b => new ByteWrap(b);

        private readonly byte _b;
        public ByteWrap(byte b) { _b = b;}

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_b);
        }
    }

    public readonly struct UInt16Wrap : ISerialize
    {
        public static readonly Func<ushort, UInt16Wrap> Ctor = i => new UInt16Wrap(i);

        private readonly ushort _i;
        public UInt16Wrap(ushort i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct UInt32Wrap : ISerialize
    {
        public static readonly Func<uint, UInt32Wrap> Ctor = i => new UInt32Wrap(i);

        private readonly uint _i;
        public UInt32Wrap(uint i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct UInt64Wrap : ISerialize
    {
        public static readonly Func<ulong, UInt64Wrap> Ctor = i => new UInt64Wrap(i);

        private readonly ulong _i;
        public UInt64Wrap(ulong i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct SByteWrap : ISerialize
    {
        public static readonly Func<sbyte, SByteWrap> Ctor = i => new SByteWrap(i);

        private readonly sbyte _i;
        public SByteWrap(sbyte i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int16Wrap : ISerialize
    {
        public static readonly Func<short, Int16Wrap> Ctor = i => new Int16Wrap(i);

        private readonly short _i;
        public Int16Wrap(short i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int32Wrap : ISerialize
    {
        public static readonly Func<int, Int32Wrap> Ctor = i => new Int32Wrap(i);

        private readonly int _i;
        public Int32Wrap(int i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int64Wrap : ISerialize
    {
        public static readonly Func<long, Int64Wrap> Ctor = i => new Int64Wrap(i);

        private readonly long _i;
        public Int64Wrap(long i) { _i = i; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct StringWrap : ISerialize
    {
        public static readonly Func<string, StringWrap> Ctor = s => new StringWrap(s);

        private readonly string _s;
        public StringWrap(string s) { _s = s; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            serializer.Serialize(_s);
        }
    }

    public readonly struct ArrayWrap<T> : ISerialize
        where T : ISerialize
    {
        public readonly static Func<T[], ArrayWrap<T>> Ctor = a => new ArrayWrap<T>(a);

        private readonly T[] _a;
        public ArrayWrap(T[] a) { _a = a; }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            var enumerable = serializer.SerializeEnumerable(_a.Length);
            foreach (var item in _a)
            {
                enumerable.SerializeElement(item);
            }
            enumerable.End();
        }
    }

    public readonly struct ArrayPrimWrap<T, TWrapped> : ISerialize
        where TWrapped : ISerialize
    {
        public static Func<T[], ArrayPrimWrap<T, TWrapped>> Ctor(Func<T, TWrapped> f)
            => a => new ArrayPrimWrap<T, TWrapped>(a, f);

        private readonly T[] _a;
        private readonly Func<T, TWrapped> _f;
        public ArrayPrimWrap(T[] a, Func<T, TWrapped> f)
        {
            _a = a;
            _f = f;
        }

        void ISerialize.Serialize<TSerializer, _1, _2>(ref TSerializer serializer)
        {
            var enumerable = serializer.SerializeEnumerable(_a.Length);
            foreach (var item in _a)
            {
                enumerable.SerializeElement(_f(item));
            }
            enumerable.End();
        }
    }
}