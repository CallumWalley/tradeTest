using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UITabContainerPlanet : TabContainer
{
    [Export]
    public Planet planet;
    [Export]
    UIPanelPoolGeneral panelGeneral;
    [Export]
    UIPanelPositionFeatures panelFeatures;
    [Export]
    UIPanelPoolSupply panelSupply;
    // [Export]
    // UIPanelPoolTrade panelTrade;

    public override void _Ready()
    {
        base._Ready();

        Name = $"UIWindow-{planet.Name}";
        panelFeatures.domain = planet;
        panelGeneral.domain = planet;
        panelSupply.domain = planet;
        // panelTrade.domain = planet;

    }
}
