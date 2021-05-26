
# C#/.NET Limitations

While implementing this project I came across several C#/.NET limitations which cause material harm
to the design, meaning that the design is worse-off in some way because either the C# language or
the .NET runtime have limitations preventing the better implementation. These are recorded below as
areas of improvement for the language and the runtime.

## Cannot add interface implementations for existing types

The design of `ISerializer` includes several associated interfaces which are used
for "builders" that serialize compound structures, like user-defined types or lists.
Each of these interfaces should have default implementations for the primitive types
which delegate to the implementation for `ISerializer`, which must be provided for any
`ISerializer` implementation. For instance, `ISerializeType` is structured around the
`SerializeField` method:

```C#
public interface ISerializeType
{
    void SerializeField<T>(string name, T value) where T : ISerialize;
    ...
}
```

For fields of a primitive type, an implementation should be provided which calls `ISerializer`. For
instance, an implementor of `ISerializer` must provide an implementation of `Serialize(bool b)`:

```C#
public interface ISerializer<
    out TSerializeType
    >
    where TSerializeType : ISerializeType
{
    void Serialize(bool b);
    ...
}
```

The Serde library could easily delegate `bool`'s `ISerialize` implementation to use
the one provided for `ISerializer`:

```C#
public readonly struct Bool : ISerialize
{
    void ISerialize.Serialize<TSerializer, _>(ref TSerializer serializer)
    {
        serializer.Serialize(_b);
    }
}
```

Unfortunately, .NET does not allow implementation of interfaces for external types.
This contrasts with Rust, which allows implementations of interfaces (traits) for any
type, provided that the user defines either the type or the interface.

The result is that the C# library must provide wrapper implementations for the primitive
types and users are forced to manually wrap primitives (or have the source generator do
so for them).

## No default implementations on interfaces without boxing

The section [on interface implementations](#Cannot-add-interface-implementations-for-existing-types)
explains why it's impossible to provide full interface implementations for primitive types. One
workaround would be to try to provide overloads for the `ISerializer` companion interfaces, e.g.

```C#
public interface ISerializeType
{
    void SerializeField<T>(string name, T t) where T : ISerialize;
    void SerializeField(string name, bool b) => SerializeField(new BoolWrap(b));
    ...
}
```

Unfortunately, this doesn't work either. If `ISerializeType` is implemented by
a struct, then invoking the default implementation will cause it to be boxed and
copied. Aside from performance problems, this is incorrect if `SerializeField<T>` is
mutating, since it will mutate a copy, not the original receiver.

## No ref extension methods on unconstrained generics

Continuing from the section on [default
implementations](##No-default-implementations-on-interfaces-without-boxing), one might try using
extension methods instead. This also doesn't work, because C# requires that `ref` extensions be
value types. In this case, common implementations may be structs, but the type parameter itself is
unconstrained. Thus, the following error is produced:

```C#
public static class ISerializeExtensions
{
    // error CS8337: The first parameter of a `ref` extension method must be a value
    // type or generic type constrained to struct
    public static void SerializeField<T>(this ref T serializeType, string name, bool b)
        where T : ISerialize
}
```

Adding the `struct` constraint is not possible here, since the caller itself is an
unconstrained type parameter, disconnecting the implementation of the serializer from
the user of the serialization library.


## No ref fields

The problem sounds simple, but there isn't one simple fix that will enable all
functionality. The problem is this: the design of the serializer is to produce
implementations of "helper" interfaces like `ISerializeType` when serializing
composite serializable values. This allows the serialization logic to keep state
specific for each composite value, which is useful both for componentization and
reducing the size of the state that needs to be held during recursive serialization
calls. An implementation looks like this:

```C#
struct Impl : ISerializer<SerializeType>
{
    public SerializeType SerializeType(string name, int numFields)
    {
        _writer.Write('{');
        return new SerializeType(this);
    }
}
struct SerializeType : ISerializeType
{
    private Impl _impl; // should be a ref
    public SerializeType(ref Impl impl)
    {
        _impl = impl; // copies
    }
}
```

The important part here is that the serializer returns a new type, which takes as a parameter a
reference to `this`. The reference to `this` is necessary because the serializer implementation is
responsible for maintaining the state of the serialization output, not just the state of the current
value being serialized. However, since `Impl` is a struct, this reference creates a copy, instead of
reference. The result is that all mutable state inside `Impl` needs to be stored as reference types,
in case the `SerializeType` calls need to mutate it.

At the simplest level, being able to store a `ref` field would solve the problem, but if done using
`ref struct`s it would also require `ref struct`s to be substitutable for generic parameters, since
the `ISerializer<ISerializeType>` specification requires it.
