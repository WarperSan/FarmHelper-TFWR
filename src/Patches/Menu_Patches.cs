using System;
using HarmonyLib;
using ModHelper.API.UI;
using ModHelper.UI;

namespace ModHelper.Patches;

[HarmonyPatch(typeof(Menu))]
internal static class Menu_Patches
{
    [HarmonyPatch(nameof(Menu.Open), new Type[]{})]
    [HarmonyPrefix]
    public static void Open()
    {
        PluginMenu.CloseAll();
        PluginMenu.Open<TitleMenu>();
    }
}