using HarmonyLib;

namespace CobaltCoreSeeded.MapRoutePatches;

[HarmonyPatch(typeof(MapRoute), nameof(MapRoute.OnClickDestination))]
public class OnClickDestinationPatch
{
    public static List<(double, Vec v)> Movements = new();

    private static void Prefix(G g, Vec key)
    {
        Movements.Add(
            (
                g.state.storyVars.runTimer,
                key
            )
        );
    }
}