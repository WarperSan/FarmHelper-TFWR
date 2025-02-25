using BepInEx.Bootstrap;
using FarmHelper.API.Interfaces;
using FarmHelper.API.UI;
using HarmonyLib;

namespace FarmHelper.Patches;

[HarmonyPatch(typeof(Menu), nameof(Menu.Start), [])]
internal static class MenuStart
{
    private static void Prefix()
    {
        PluginMenu.ClearAll();
        foreach (var entry in Chainloader.PluginInfos)
        {
            if (entry.Value.Instance is IToggleable toggleable)
                toggleable.OnAdd();
        }
    }
}