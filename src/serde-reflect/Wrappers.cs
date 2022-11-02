
using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Serde;

namespace Serde.Reflect;

internal static class Wrappers
{
    public static Func<object, ISerialize>? TryGetWrapper(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type t)
    {
        if (t.GetInterface("Serde.ISerialize") is not null)
        {
            return o => (ISerialize)o;
        }
        if (TryGetPrimitiveWrapper(t) is {} prim)
        {
            return prim;
        }
        if (t.IsEnum)
        {
            var wrapper = TryGetPrimitiveWrapper(t.GetEnumUnderlyingType());
            if (wrapper is null)
            {
                throw new InvalidOperationException($"Unrecognized enum underlying value '{t.GetEnumUnderlyingType()}'");
            }
            return o => new EnumWrap(t, wrapper, o);
        }
        if (t.IsArray)
        {
            var wrap = GetWrapperOrThrow(t.GetElementType());
            return o => new ArrayWrap(wrap, (Array)o);
        }
        if (t.GetInterface("System.Collections.Generic.IEnumerable`1") is not null)
        {
            var wrap = GetWrapperOrThrow(t.GenericTypeArguments[0]);
            return o => new EnumerableWrap(wrap, (IEnumerable)o);
        }
        return null;
    }

    private static Func<object, ISerialize> GetWrapperOrThrow(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type t)
    {
        if (TryGetWrapper(t) is {} wrap)
        {
            return wrap;
        }
        throw new InvalidOperationException($"Could not find wraper for type '{t}'");
    }

    private static Func<object, ISerialize>? TryGetPrimitiveWrapper(Type t)
    {
        if (t == typeof(bool))
        {
            return o => new BoolWrap((bool)o);
        }
        if (t == typeof(string))
        {
            return o => new StringWrap((string)o);
        }
        if (t == typeof(char))
        {
            return o => new CharWrap((char)o);
        }
        if (t == typeof(sbyte))
        {
            return o => new SByteWrap((sbyte)o);
        }
        if (t == typeof(Int16))
        {
            return o => new Int16Wrap((Int16)o);
        }
        if (t == typeof(Int32))
        {
            return o => new Int32Wrap((Int32)o);
        }
        if (t == typeof(Int64))
        {
            return o => new Int64Wrap((Int64)o);
        }
        if (t == typeof(byte))
        {
            return o => new ByteWrap((byte)o);
        }
        if (t == typeof(UInt16))
        {
            return o => new UInt16Wrap((UInt16)o);
        }
        if (t == typeof(UInt32))
        {
            return o => new UInt32Wrap((UInt32)o);
        }
        if (t == typeof(UInt64))
        {
            return o => new UInt64Wrap((UInt64)o);
        }
        return null;
    }
}

internal sealed class NullWrap : ISerialize
{
    void ISerialize.Serialize(ISerializer serializer)
    {
        serializer.SerializeNull();
    }
}

internal class ArrayWrap : ISerialize
{
    private readonly Func<object, ISerialize> _elemWrap;
    private readonly Array _a;

    public ArrayWrap(Func<object, ISerialize> elemWrap, Array a)
    {
        _elemWrap = elemWrap;
        _a = a;
    }

    void ISerialize.Serialize(ISerializer serializer)
    {
        var e = serializer.SerializeEnumerable(_a.GetType().Name, _a.Length);
        foreach (var elem in _a)
        {
            e.SerializeElement(_elemWrap(elem));
        }
        e.End();
    }
}

internal sealed class EnumerableWrap : ISerialize
{
    private readonly Func<object, ISerialize> _elemWrap;
    private readonly IEnumerable _enumerable;

    public EnumerableWrap(Func<object, ISerialize> elemWrap, IEnumerable e)
    {
        _elemWrap = elemWrap;
        _enumerable = e;
    }

    void ISerialize.Serialize(ISerializer serializer)
    {
        var e = serializer.SerializeEnumerable(_enumerable.GetType().Name, length: null);
        foreach (var elem in _enumerable)
        {
            e.SerializeElement(_elemWrap(elem));
        }
        e.End();
    }
}

internal sealed class EnumWrap : ISerialize
{
    private readonly Type _type;
    private readonly Func<object, ISerialize> _wrap;
    private readonly object _o;

    public EnumWrap(Type type, Func<object, ISerialize> wrap, object o)
    {
        _wrap = wrap;
        _type = type;
        _o = o;
    }

    void ISerialize.Serialize(ISerializer serializer)
    {
        var valueName = _type.GetEnumName(_o);
        serializer.SerializeEnumValue(_type.Name, valueName, _wrap(_o));
    }
}