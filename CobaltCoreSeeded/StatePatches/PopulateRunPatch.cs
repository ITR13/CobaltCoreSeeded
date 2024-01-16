using HarmonyLib;

namespace CobaltCoreSeeded.StatePatches;

[HarmonyPatch(typeof(State), nameof(State.PopulateRun))]
public static class PopulateRunPatch
{
    private static void Prefix()
    {
        // Here to prevent inlining of PopulateRun
    }
}