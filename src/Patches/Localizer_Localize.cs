using AgriCore.Helpers;
using HarmonyLib;

namespace AgriCore.Patches;

[HarmonyPatch(typeof(Localizer), nameof(Localizer.Localize), typeof(string))]
internal static class LocalizerLocalize
{
    private static bool Prefix(string key, ref string __result)
    {
        var value = LocalizerHelper.GetText(Localizer.Lang, key);

        if (value == null)
            return true;

        __result = value;
        return false;
    }
}