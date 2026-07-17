using System.Runtime.CompilerServices;
using VerifyTests;

internal static class ModuleInit
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}
