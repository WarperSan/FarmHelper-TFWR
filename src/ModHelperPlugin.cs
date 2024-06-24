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
    private void OnAwake()
    {
        var harmony = new Harmony(Info.Metadata.GUID);
        harmony.PatchAll();
        //Harmony.UnpatchSelf();
    }

    private void Start()
    {
        PluginsPage.Create();
        
        //PrintAll();
        CodeHelper.AddFunction(Pow);
        CodeHelper.AddFunction(FloorToInt);
        //CodeHelper.AddCodeColor(@"intFloor(?=\(.*?\))", "#33b5aa", true);
        //CodeHelper.AddCodeColor(@"pow(?=\(.*?\))", "#33b5aa", true);
    }

    [PyFunction("pow")]
    private double Pow(Interpreter interpreter, PyNumber a, PyNumber b)
    {
        var result = Mathf.Pow((float)a.num, (float)b.num);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }

    [PyFunction("floor")]
    private double FloorToInt(Interpreter interpreter, PyNumber x)
    {
        var result = Mathf.FloorToInt((float)x.num);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }
}