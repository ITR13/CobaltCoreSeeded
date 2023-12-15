using System.Reflection;
using System.Reflection.Emit;
using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace CobaltCoreSeeded;

[HarmonyPatch(typeof(NewRunOptions), nameof(NewRunOptions.OnMouseDown))]
public static class OnMouseDownPatch
{
    internal static bool SeedCurrentlyDown { get; set; }

    public static void Postfix(G g, Box b)
    {
        if (b.key != Main.SEEDBOX_UK_VALUE) return;
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