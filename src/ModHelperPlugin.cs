using BepInEx;
using ModHelper.API;
using ModHelper.Extensions;
using ModHelper.Helpers;
using UnityEngine;

namespace ModHelper;

[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
public class ModHelperPlugin : FarmPlugin
{
    /// <inheritdoc />
    public override string Author => "WarperSan";

    protected override void OnAwake()
    {
        Harmony.PatchAll();
        //Harmony.UnpatchSelf();
    }

    private void Start()
    {
        ModsPage.Create();

        //PrintAll();
        CodeHelper.AddFunction(Pow);
    }

    [PyFunction("pow")]
    private double Pow(Interpreter interpreter, PyNumber a, PyNumber b)
    {
        var result = Mathf.Pow((float)a.num, (float)b.num);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }
    
    private void PrintAll()
    {
        foreach (var variable in GameObject.FindObjectsOfType<Transform>())
        {
            if (variable.parent == null)
                variable.PrintDeeper();
        }
    }
}