using BepInEx;
using ModHelper.API;

namespace ModHelper;

[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
public class ModHelperPlugin : FarmPlugin<ModHelperPlugin>
{
    protected override void OnAwake()
    {
        Harmony.PatchAll();
    }
}
