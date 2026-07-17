using System.Threading.Tasks;
using VerifyTests;

namespace VerifyNXTest
{
    /// <summary>
    /// Minimal, test-framework-agnostic replacement for the <c>Verify.XunitV3</c> integration.
    /// The Verify engine (<see cref="InnerVerifier"/>) lives in the framework-agnostic core; the
    /// only thing a framework package supplies is the snapshot identity (type/method name) and the
    /// source file. NXTest has no ambient test context, so callers thread those values in explicitly.
    /// </summary>
    public static class Verifier
    {
        public static SettingsTask Verify(
            object? target,
            VerifySettings settings,
            string typeName,
            string methodName,
            string sourceFile
        ) =>
            new SettingsTask(
                settings,
                async resolvedSettings =>
                {
                    using var verifier = new InnerVerifier(
                        sourceFile,
                        resolvedSettings,
                        typeName,
                        methodName,
                        methodParameters: null,
                        new PathInfo()
                    );
                    return await verifier.Verify(target);
                }
            );
    }
}
