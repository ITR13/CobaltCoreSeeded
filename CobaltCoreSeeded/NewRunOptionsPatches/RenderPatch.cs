using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Input;
using TextCopy;

namespace CobaltCoreSeeded.NewRunOptionsPatches;

[HarmonyPatch(typeof(NewRunOptions), nameof(NewRunOptions.Render))]
public static class RenderPatch
{
    private static G? _g;
    private static NewRunOptions? _instance;
    private static bool _gErroredOnce;
    private static MethodInfo _renderSeed = SymbolExtensions.GetMethodInfo(() => RenderSeed());

    private static void Prefix(NewRunOptions __instance, G g)
    {
        _g = g;
        _instance = __instance;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var foundString = false;
        var foundPop = false;
        foreach (var instruction in instructions)
        {
            yield return instruction;

            if (foundPop) continue;

            if (!foundString)
            {
                if (instruction.LoadsConstant("RANDOMIZE"))
                {
                    foundString = true;
                    Main.Log(LogLevel.Information, "Found RANDOMIZE string");
                }

                continue;
            }

            if (instruction.opcode == OpCodes.Pop)
            {
                yield return new CodeInstruction(OpCodes.Call, _renderSeed);
                foundPop = true;
                Main.Log(LogLevel.Information, "Found Pop after RANDOMIZE");
            }
        }

        if (foundPop is false) Main.Log(LogLevel.Error, "Cannot find seed field entry point");
    }

    private static void RenderSeed()
    {
        if (_g == null)
        {
            if (_gErroredOnce) return;
            _gErroredOnce = true;
            Main.Log(LogLevel.Error, "G was not set??");

            return;
        }

        var position = new Vec(0.0, 2 * 27.0);

        foreach (var startKey in new[] { Keys.D0, Keys.NumPad0 })
        {
            for (uint i = 0; i < 10; i++)
            {
                var key = startKey + (int)i;
                if (!Input.GetKeyDown(key)) continue;
                if (Main.Seed == null)
                {
                    OnMouseDownPatch.SeedCurrentlyDown = true;
                    Main.Seed = 0;
                }

                Main.Seed = Main.Seed * 10 + i;
            }
        }

        if (Input.GetKeyDown(Keys.Back))
        {
            Main.Seed /= 10;
            if (Main.Seed == 0) Main.Seed = null;
        }

        if (Input.GetKeyHeld(Keys.LeftControl))
        {
            if (Input.GetKeyHeld(Keys.V))
            {
                var clipboardText = ClipboardService.GetText() ?? "";
                if (uint.TryParse(clipboardText, out var newSeed))
                {
                    Main.Seed = newSeed;
                    OnMouseDownPatch.SeedCurrentlyDown = true;
                }
            }

            if (Main.Seed != null && Input.GetKeyHeld(Keys.C))
            {
                var clipboardText = Main.Seed.ToString() ?? "";
                ClipboardService.SetText(clipboardText);
            }
        }

        var active = OnMouseDownPatch.SeedCurrentlyDown;
        var text = Main.Seed?.ToString() ?? "Seed";

        SharedArt.ButtonText(
            _g,
            position,
            Main.SEEDBOX_UK_VALUE,
            text,
            inactive: false,
            autoFocus: false,
            gamepadUntargetable: true,
            hasDownState: true,
            showAsPressed: active,
            onMouseDown: _instance
        );
    }
}