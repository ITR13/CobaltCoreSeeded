using CobaltCoreSeeded.MapRoutePatches;
using CobaltCoreSeeded.ParticleBurstsPatches;
using HarmonyLib;

namespace CobaltCoreSeeded.RunSummaryPatches;

[HarmonyPatch(typeof(RunSummary), nameof(RunSummary.Save))]
public static class SavePatch
{
    private static void Prefix(RunSummary __instance, int slot)
    {
        var toSave = new Timing
        {
            Explosions = ShipExplosionPatch.Explosions,
            MapMovements = OnClickDestinationPatch
                .Movements
                .Select(v => new DestinationTiming(v))
                .ToList(),
        };
        var path = Path.Combine(
            State.GetSlotPath(slot),
            "RunTimings",
            $"RunTiming_{__instance.timestamp}.json.gz"
        );
        Storage.Save(toSave, path);
    }

    [Serializable]
    private struct Timing
    {
        public List<double> Explosions;
        public List<DestinationTiming> MapMovements;
    }

    [Serializable]
    private struct DestinationTiming
    {
        public double Time;
        public Vec Position;

        public DestinationTiming((double t, Vec v) valueTuple)
        {
            Time = valueTuple.t;
            Position = valueTuple.v;
        }
    }
}