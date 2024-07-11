using FarmHelper.API.UI;
using UnityEngine;

namespace FarmHelper.UI;

internal class TitleMenu : PluginMenu
{
    /// <inheritdoc />
    public override string DisplayName => "Title";

    /// <inheritdoc />
    protected override GameObject Create() => FindObjectOfType<Menu>().titlePage;
}