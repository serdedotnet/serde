//HintName: RgbWrap.ISerializeWrap.cs

partial record struct RgbWrap : Serde.ISerializeWrap<Rgb, RgbWrap>
{
    static RgbWrap Serde.ISerializeWrap<Rgb, RgbWrap>.Create(Rgb value) => new(value);
}