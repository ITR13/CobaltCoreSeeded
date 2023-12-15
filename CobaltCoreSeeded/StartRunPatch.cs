using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace CobaltCoreSeeded;

[HarmonyPatch(typeof(NewRunOptions), nameof(NewRunOptions.StartRun))]
public static class StartRunPatch
{
    private static void Prefix(ref uint? seed)
    {
        if (Main.Seed == null) return;
        if (seed != null)
        {
            Main.Log(
                LogLevel.Warning,
                $"Not setting seed to {Main.Seed} because StartRun was called with {seed}"
            );

            return;
        }

        seed = Main.Seed;
        Main.Log(LogLevel.Information, $"Starting run with seed {seed}");
    }
}