using HarmonyLib;

namespace CobaltCoreSeeded.ParticleBurstsPatches;

[HarmonyPatch(typeof(ParticleBursts), nameof(ParticleBursts.ShipExplosion))]
public static class ShipExplosionPatch
{
    public static List<double> Explosions = new();
    
    private static void Prefix(State s)
    {
        Explosions.Add(s.storyVars.runTimer);
    }
}