//HintName: Some.Nested.Namespace.ColorIntProxy.cs


namespace Some.Nested.Namespace;

sealed partial class ColorIntProxy
{
    public static readonly ColorIntProxy Instance = new();
    private ColorIntProxy() { }
}