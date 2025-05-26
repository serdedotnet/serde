
namespace Serde.Test;

partial struct MaxSizeType : Serde.ISerdeProvider<Serde.Test.MaxSizeType, Serde.Test.MaxSizeType._SerdeObj, Serde.Test.MaxSizeType>
{
    static Serde.Test.MaxSizeType._SerdeObj global::Serde.ISerdeProvider<Serde.Test.MaxSizeType, Serde.Test.MaxSizeType._SerdeObj, Serde.Test.MaxSizeType>.Instance { get; }
        = new Serde.Test.MaxSizeType._SerdeObj();
}
