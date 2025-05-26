//HintName: Color.ISerdeProvider.cs
partial record Color : Serde.ISerdeProvider<Color, ColorSerdeObj, Color>
{
    static ColorSerdeObj global::Serde.ISerdeProvider<Color, ColorSerdeObj, Color>.Instance { get; }
        = new ColorSerdeObj();
}
