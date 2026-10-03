//HintName: Shape._m_InstallProxy.IDeserializeProvider.g.cs
partial record Shape
{
    partial class _m_InstallProxy : Serde.IDeserializeProvider<Shape.Install>
    {
        static global::Serde.IDeserialize<Shape.Install> global::Serde.IDeserializeProvider<Shape.Install>.Instance { get; }
            = new Shape._m_InstallProxy._DeObj();
    }
}
