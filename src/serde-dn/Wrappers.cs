// Contains implementations of data interfaces for core types

using System;
using System.Diagnostics;

namespace Serde
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    [Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
    public sealed class SerdeWrapAttribute : Attribute
    { 
        public SerdeWrapAttribute(Type wrapper)
        {
            Wrapper = wrapper;
        }
        public Type Wrapper { get; }
    }

    public interface IWrap<T, TWrap> where TWrap : ISerialize
    {
        TWrap Create(T t); // Should be abstract static
    }

    public readonly struct BoolWrap : ISerialize, ISerializeStatic, IWrap<bool, BoolWrap>
    {
        public BoolWrap Create(bool t) => new BoolWrap(t);

        private readonly bool _b;
        public BoolWrap(bool b) { _b = b; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_b);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_b);
        }
    }

    public readonly struct CharWrap : ISerialize, ISerializeStatic, IWrap<char, CharWrap>
    {
        public CharWrap Create(char c) => new CharWrap(c);

        private readonly char _c;
        public CharWrap(char c) { _c = c; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_c);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_c);
        }
    }

    public readonly struct ByteWrap : ISerialize, ISerializeStatic, IWrap<byte, ByteWrap>
    {
        public ByteWrap Create(byte b) => new ByteWrap(b);

        private readonly byte _b;
        public ByteWrap(byte b) { _b = b;}

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_b);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_b);
        }
    }

    public readonly struct UInt16Wrap : ISerialize, ISerializeStatic, IWrap<ushort, UInt16Wrap>
    {
        public UInt16Wrap Create(ushort i) => new UInt16Wrap(i);

        private readonly ushort _i;
        public UInt16Wrap(ushort i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct UInt32Wrap : ISerialize, ISerializeStatic, IWrap<uint, UInt32Wrap>
    {
        public UInt32Wrap Create(uint i) => new UInt32Wrap(i);

        private readonly uint _i;
        public UInt32Wrap(uint i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct UInt64Wrap : ISerialize, ISerializeStatic, IWrap<ulong, UInt64Wrap>
    {
        public UInt64Wrap Create(ulong i) => new UInt64Wrap(i);

        private readonly ulong _i;
        public UInt64Wrap(ulong i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct SByteWrap : ISerialize, ISerializeStatic, IWrap<sbyte, SByteWrap>
    {
        public SByteWrap Create(sbyte i) => new SByteWrap(i);

        private readonly sbyte _i;
        public SByteWrap(sbyte i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int16Wrap : ISerialize, ISerializeStatic, IWrap<short, Int16Wrap>
    {
        public Int16Wrap Create(short i) => new Int16Wrap(i);

        private readonly short _i;
        public Int16Wrap(short i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int32Wrap : ISerialize, ISerializeStatic, IWrap<int, Int32Wrap>
    {
        public Int32Wrap Create(int i) => new Int32Wrap(i);

        private readonly int _i;
        public Int32Wrap(int i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct Int64Wrap : ISerialize, ISerializeStatic, IWrap<long, Int64Wrap>
    {
        public Int64Wrap Create(long i) => new Int64Wrap(i);

        private readonly long _i;
        public Int64Wrap(long i) { _i = i; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_i);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_i);
        }
    }

    public readonly struct StringWrap : ISerialize, ISerializeStatic, IWrap<string, StringWrap>
    {
        public StringWrap Create(string s) => new StringWrap(s);

        private readonly string _s;
        public StringWrap(string s) { _s = s; }

        void ISerializeStatic.Serialize<TSerializer, _1, _2, _3>(ref TSerializer serializer)
        {
            serializer.Serialize(_s);
        }

        void ISerialize.Serialize(ISerializer serializer)
        {
            serializer.Serialize(_s);
        }
    }
}