using BepInEx;
using ModHelper.API;

namespace ModHelper;

[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
public class Plugin : FarmPlugin<Plugin>
{
    protected override void OnAwake()
    {
        Harmony.PatchAll();
    }
}
