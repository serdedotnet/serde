
namespace Serde;

public interface ISerdeInfoProvider
{
    ISerdeInfo SerdeInfo { get; }
}

public static class SerdeInfoProvider
{
    public static ISerdeInfo GetInfo<TProvider>(TProvider provider)
        where TProvider : ISerdeInfoProvider
        => provider.SerdeInfo;
    public static ISerdeInfo GetSerializeInfo<T, TProvider>()
        where TProvider : ISerializeProvider<T>
        => TProvider.Instance.SerdeInfo;
    public static ISerdeInfo GetSerializeInfo<T>()
        where T : ISerializeProvider<T>
        => T.Instance.SerdeInfo;
    public static ISerdeInfo GetDeserializeInfo<T, TProvider>()
        where TProvider : IDeserializeProvider<T>
        => TProvider.Instance.SerdeInfo;
    public static ISerdeInfo GetDeserializeInfo<T>()
        where T : IDeserializeProvider<T>
        => T.Instance.SerdeInfo;
}