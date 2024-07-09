using ModHelper.API.UI;
using UnityEngine;

namespace ModHelper.UI;

internal class TitleMenu : PluginMenu
{
    /// <inheritdoc />
    protected override GameObject Create() => FindObjectOfType<Menu>().titlePage;
}