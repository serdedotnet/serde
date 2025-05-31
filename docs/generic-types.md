# Handling generic types

For most types, adding Serde.NET support is as easy as implementing the `ISerialize` and `IDeserialize` interfaces, which the source generator can do for you.

Generic types are usually not so easy, because of a specific restriction in the .NET type system.

Let's say you have a simple custom generic list type, like `MyList<T>`. Serde already provides built-in support for automatically wrapping Lists using [prewritten wrappers](https://github.com/serdedotnet/serde/blob/main/src/serde/Wrappers.List.cs), but you might have a customization you want to provide. In that case you might try implementing `ISerialize` and `IDeserialize` yourself, but run into a problem -- for `MyList<T>` to be serializable, all its elements (`T`) must be serializable.

The natural inclination would be to add a constraint to the `MyList<T>` definition: `MyList<T> where T : ISerialize<T>, IDeserialize<T>`. Unfortunately, that won't work. First, it would create a requirement that you could only put serializable elements into the `MyList<T>` type. However, that's not the contract you want to provide. You want to support serialization in the case that all types are serializable, but you don't want to _require_ that all types be serializable. Second, even the primitive types, like `int` and `string`, don't implement `ISerialize<T>` or `IDeserialize<T>` directly -- they use wrappers.

So what's the solution? Using a wrapper type instead. Rather than implement serialization on `MyList<T>` itself, define a wrapper type for Serde, then point to that wrapper type from the `MyList<T>` definition. The information at [Wrappers](./generator/wrappers.md) is very useful as background.

One important point is that generic wrappers are slightly different from regular wrappers. To be more flexible they provide serialization and deserialization separately, and therefore are implemented using a different pattern. They start with a static class at the top level, and feature a `SerializeImpl` and `DeserializeImpl` nested beneath. For `MyList<T>` this would look like,

```C#
public static class MyListSerdeWrap
{
    public readonly record struct SerializeImpl<T, TWrap>(MyList<T> Value) : ISerialize, ISerialize<MyList<T>>
        where TWrap : struct, ISerialize, ISerialize<T>, ISerializeWrap<T, TWrap>
    {
        ...
    }
    public readonly record struct DeserializeImpl<T, TWrap> : IDeserialize<MyList<T>>
        where TWrap : IDeserialize<T>
    {
        ...
    }
}
```

Note that each nested class takes at least two type parameters. The first type parameter is for the type parameter of the original type. The second is for the wrapper that might be needed for previous type parameter. The rule is that for `n` type parameters on the original type, you'll need `2n` type paramaeters for the wrapper type.

The implementation of the wrapper is otherwise standard. For collections, you can reference the [prewritten List and Dictionary wrappers](https://github.com/serdedotnet/serde/blob/main/src/serde/Wrappers.List.cs) for implementation tips.