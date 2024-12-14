
namespace Serde;

public interface ISerdeInfoProvider
{
    abstract static ISerdeInfo SerdeInfo { get; }
}

public static class SerdeInfoProvider
{
    public static ISerdeInfo GetInfo<T>() where T : ISerdeInfoProvider
        => T.SerdeInfo;
}