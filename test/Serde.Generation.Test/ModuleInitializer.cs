
using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;

internal static class ModuleInit
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}