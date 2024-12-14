//HintName: Some.Nested.Namespace.ColorByteProxy.cs

namespace Some.Nested.Namespace
{
    sealed partial class ColorByteProxy
    {
        public static readonly ColorByteProxy Instance = new();
        private ColorByteProxy()
        {
        }
    }
}