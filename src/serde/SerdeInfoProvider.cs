
namespace Serde;

public interface ISerdeInfoProvider
{
    abstract static SerdeInfo SerdeInfo { get; }
}

public static class SerdeInfoProvider
{
    public static SerdeInfo GetInfo<T>() where T : ISerdeInfoProvider
        => T.SerdeInfo;
}