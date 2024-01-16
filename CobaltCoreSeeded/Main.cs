using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nickel;

namespace CobaltCoreSeeded;

// ReSharper disable once ClassNeverInstantiated.Global
public class Main : Mod
{
    public const UK SeedboxUkValue = (UK)393964740;
    public static Action<LogLevel, string> Log = (_, _) => { };
    public static uint? Seed { get; internal set; }
    
    public Main(ILogger logger)
    {
        Log = ((level, s) => logger.Log(level, s));
        Log(LogLevel.Information, "Loaded Seeded!");
        var harmony = new Harmony("com.itr.cobaltcore.seeded");
        harmony.PatchAll();
    }
}