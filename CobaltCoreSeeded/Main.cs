using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace CobaltCoreSeeded;

public class Main : IModManifest
{
    public const UK SEEDBOX_UK_VALUE = (UK)393964740;
    
    public IEnumerable<DependencyEntry> Dependencies => Array.Empty<DependencyEntry>();
    public DirectoryInfo? GameRootFolder { get; set; }
    public ILogger? Logger { get; set; }
    public DirectoryInfo? ModRootFolder { get; set; }
    public string Name => "ITR's Seeded Run";

    public static Action<LogLevel, string> Log = (_, _) => { };
    public static uint? Seed { get; internal set; }

    public void BootMod(IModLoaderContact contact)
    {
        if (Logger == null)
        {
            Log = ((level, s) => Console.WriteLine($"[{level}] {s}"));
        }
        else
        {
            Log = ((level, s) => Logger.Log(level, s));
        }

        var harmony = new Harmony("com.itr.cobaltcore.savefix");
        harmony.PatchAll();
    }
}