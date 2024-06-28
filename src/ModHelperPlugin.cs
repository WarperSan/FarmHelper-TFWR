using BepInEx;
using HarmonyLib;
using ModHelper.API;
using ModHelper.Helpers;
using UnityEngine;
// ReSharper disable StringLiteralTypo

namespace ModHelper;

/// <summary>
/// Plugin made to help other plugins 
/// </summary>
[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
[FarmInfo("WarperSan")]
public class ModHelperPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony(Info.Metadata.GUID);
        harmony.PatchAll();
        //Harmony.UnpatchSelf();
    }

    private void Start()
    {
        PluginsPage.Create();
        
        //PrintAll();
        FuncHelper.AddAll<ModHelperFunctions>();
        
        //ColorHelper.Add(@"floor(?=\(.*?)", "#33b5aa", true);
        //ColorHelper.Add(@"pow(?=\(.*?)", "#33b5aa", true);
        //ColorHelper.Add(@"round(?=\(.*?)", "#33b5aa", true);

        // typeof(Localizer).GetStaticField<Dictionary<string, string>>("language")
        //     .Add("code_tooltip_pow", test);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Saver.Inst.mainFarm.leaderboardManager.StartLeaderboardRun();
            Saver.Inst.mainFarm.leaderboardManager.StopLeaderboardRun(false);
        }
    }
}