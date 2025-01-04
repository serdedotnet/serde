//HintName: Test.ChannelProxy.cs


namespace Test;

sealed partial class ChannelProxy
{
    public static readonly ChannelProxy Instance = new();
    private ChannelProxy() { }
}