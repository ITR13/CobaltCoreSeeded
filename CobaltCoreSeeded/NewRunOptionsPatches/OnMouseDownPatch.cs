using FSPRO;
using HarmonyLib;

namespace CobaltCoreSeeded.NewRunOptionsPatches;

[HarmonyPatch(typeof(NewRunOptions), nameof(NewRunOptions.OnMouseDown))]
public static class OnMouseDownPatch
{
    internal static bool SeedCurrentlyDown { get; set; }

    public static void Postfix(G g, Box b)
    {
        if (b.key != Main.SeedboxUkValue) return;
        Audio.Play(Event.Click);

        if (SeedCurrentlyDown)
        {
            SeedCurrentlyDown = false;
            Main.Seed = null;
            return;
        }

        SeedCurrentlyDown = true;
        Main.Seed = (uint)Mutil.NextRandInt();
    }
}