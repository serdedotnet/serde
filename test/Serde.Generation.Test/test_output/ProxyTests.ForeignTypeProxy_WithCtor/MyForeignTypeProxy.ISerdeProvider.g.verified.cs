//HintName: MyForeignTypeProxy.ISerdeProvider.g.cs
partial record struct MyForeignTypeProxy : Serde.ISerdeProvider<MyForeignTypeProxy, MyForeignTypeProxy._SerdeObj, MyForeignType>
{
    static MyForeignTypeProxy._SerdeObj global::Serde.ISerdeProvider<MyForeignTypeProxy, MyForeignTypeProxy._SerdeObj, MyForeignType>.Instance { get; }
        = new MyForeignTypeProxy._SerdeObj();
}
