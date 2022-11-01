using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Serde.Reflect;

public interface IReflectionSerialize<
    [DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicFields
        | DynamicallyAccessedMemberTypes.PublicProperties)] T> : ISerialize
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
        var nullContext = new NullabilityInfoContext();
        foreach (var p in props)
        {
            var nullability = nullContext.Create(p);
            HandleMember(p, p.PropertyType, nullability, (m, typedThis) => ((PropertyInfo)m).GetMethod!.Invoke(typedThis, Array.Empty<object>()));
        }
        foreach (var f in fields)
        {
            var nullability = nullContext.Create(f);
            HandleMember(f, f.FieldType, nullability, (m, typedThis) => ((FieldInfo)m).GetValue(typedThis));
        }
        serializeType.End();

        return;

        void HandleMember(
            MemberInfo m,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type t,
            NullabilityInfo nullability,
            Func<MemberInfo, T, object?> getter)
        {
            var o = getter(m, typedThis);
            if (o is null && nullability.ReadState != NullabilityState.Nullable)
            {
                // Default behavior is to not serialize null
                return;
            }

            if (Wrappers.TryGetWrapper(t) is {} wrap)
            {
                serializeType.SerializeField(m.Name, wrap(o));
                return;
            }

            throw new InvalidOperationException($"Type '{o!.GetType()}' of member '{m.DeclaringType}.{m.Name}' does not implement ISerialize."
                + " All serialized types must implement ISerialize or have a wrapper that implements ISerialize.");
        }
    }
}
