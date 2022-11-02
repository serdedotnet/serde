
using Serde;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Serde.Reflect;

public interface IReflectionDeserialize<
    [DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicFields |
        DynamicallyAccessedMemberTypes.PublicProperties |
        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T> : IDeserialize<T>
{
    private const BindingFlags PublicInstanceFlags = BindingFlags.Public | BindingFlags.Instance;
    static T IDeserialize<T>.Deserialize<D>(ref D deserializer)
    {
        var props = typeof(T).GetProperties(PublicInstanceFlags);
        var fields = typeof(T).GetFields(PublicInstanceFlags);
        int total = props.Length + fields.Length;
        var names = new string[total];
        var nameToMember = new Dictionary<string, MemberInfo>();
        int i = 0;
        foreach (var p in props)
        {
            names[i++] = p.Name;
            nameToMember.Add(p.Name, p);
        }
        foreach (var f in fields)
        {
            names[i++] = f.Name;
            nameToMember.Add(f.Name, f);
        }
        return deserializer.DeserializeType<T, Visitor>(typeof(T).ToString(), names, new Visitor(nameToMember));
    }

    private sealed class Visitor : IDeserializeVisitor<T>
    {
        private readonly Dictionary<string, MemberInfo> _nameToMember;
        public Visitor(Dictionary<string, MemberInfo> nameToMember)
        {
            _nameToMember = nameToMember;
        }
        public string ExpectedTypeName => typeof(T).ToString();

        T IDeserializeVisitor<T>.VisitDictionary<D>(ref D d)
        {
            var memberValues = new Dictionary<string, object>();
            while (d.TryGetNextKey<string, StringWrap>(out var fieldName))
            {
                var prop = typeof(T).GetProperty(fieldName);
                if (prop is not null)
                {
                }
                var val = d.GetNextValue<object, ObjectWrap>();
                memberValues.Add(fieldName, val);
            }
            var ctor = typeof(T).GetConstructor(Array.Empty<Type>());
            if (ctor is null)
            {
                throw new InvalidOperationException($"Type to deserialize '{typeof(T)}' does not have a parameterless public constructor");
            }
            var result = ctor.Invoke(Array.Empty<object>());
            foreach (var (name, val) in memberValues)
            {
                var member = _nameToMember[name];
                if (member is PropertyInfo p)
                {
                    p.SetValue(result, val);
                }
                if (member is FieldInfo f)
                {
                    f.SetValue(result, val);
                }
            }
            return (T)result;
        }
    }
}

internal struct ObjectWrap : IDeserialize<object>
{
    static object IDeserialize<object>.Deserialize<D>(ref D deserializer)
    {
        return deserializer.DeserializeAny<object, Visitor>(new Visitor());
    }

    private sealed class Visitor : IDeserializeVisitor<object>
    {
        public string ExpectedTypeName => "object";

        object IDeserializeVisitor<object>.VisitBool(bool b) => b;
        object IDeserializeVisitor<object>.VisitByte(byte b) => b;
        object IDeserializeVisitor<object>.VisitFloat(float f) => f;
        object IDeserializeVisitor<object>.VisitString(string s) => s;
        object IDeserializeVisitor<object>.VisitI64(long i64) => i64;
    }
}