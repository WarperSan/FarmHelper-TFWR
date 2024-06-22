using BepInEx;
using ModHelper.API;
using ModHelper.Extensions;
using UnityEngine;

namespace ModHelper;

[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
public class ModHelperPlugin : FarmPlugin<ModHelperPlugin>
{
    /// <inheritdoc />
    public override string Author => "WarperSan";

    protected override void OnAwake()
    {
        Harmony.PatchAll();
    }

    private void Start()
    {
        ModsPage.Create();

        //PrintAll();
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
