using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Serde.Reflect;

public interface IReflectionSerialize<
    T> : ISerialize
    where T : IReflectionSerialize<T>
{
    private const BindingFlags PublicInstanceFlags = BindingFlags.Public | BindingFlags.Instance;

    void ISerialize.Serialize(ISerializer serializer)
    {
        var typedThis = (T)this;
        var props = typeof(T).GetProperties(PublicInstanceFlags);
        var fields = typeof(T).GetFields(PublicInstanceFlags);
        int total = props.Length + fields.Length;
        var serializeType = serializer.SerializeType(typeof(T).Name, total);
        foreach (var p in props)
        {
            HandleMember(p.PropertyType, p, (m, typedThis) => ((PropertyInfo)m).GetMethod!.Invoke(typedThis, Array.Empty<object>()));
        }
        foreach (var f in fields)
        {
            HandleMember(f.FieldType, f, (m, typedThis) => ((FieldInfo)m).GetValue(typedThis));
        }
        serializeType.End();

        return;

        void HandleMember(Type type, MemberInfo m, Func<MemberInfo, T, object?> getter)
        {
            var o = getter(m, typedThis);
            switch (o)
            {
                case null:
                    // Default behavior is to not serialize null
                    return;
                case string s:
                    var wrap = new StringWrap(s);
                    serializeType.SerializeField(m.Name, wrap);
                    break;
                default:
                    throw new InvalidOperationException($"Type '{type}' of member '{m.DeclaringType}.{m.Name}' does not implement ISerialize."
                        + " All serialized types must implement ISerialize or have a wrapper that implements ISerialize.");
            }
        }
    }
}
