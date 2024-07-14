using FarmHelper.Helpers;
using HarmonyLib;

namespace FarmHelper.Patches;

[HarmonyPatch(typeof(Localizer), nameof(Localizer.Localize), [typeof(string)])]
internal static class Localizer_Localize
{
    private static bool Prefix(string key, ref string __result)
    {
        string value = LocalizerHelper.GetText(Localizer.Lang, key);

        if (value == null)
            return true;

        __result = value;
        return false;
    }
}