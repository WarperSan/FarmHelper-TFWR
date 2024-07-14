using BepInEx.Bootstrap;
using FarmHelper.API.Interfaces;
using FarmHelper.API.UI;
using HarmonyLib;

namespace FarmHelper.Patches;

[HarmonyPatch(typeof(Menu), nameof(Menu.Start), [])]
internal static class Menu_Start
{
    private static void Prefix()
    {
        PluginMenu.ClearAll();
        foreach (var (_, info) in Chainloader.PluginInfos)
        {
            if (info.Instance is IToggleable toggleable)
                toggleable.OnAdd();
        }
    }
}