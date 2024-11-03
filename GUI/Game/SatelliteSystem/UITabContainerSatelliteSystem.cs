using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UITabContainerSatelliteSystem : TabContainer
{
    [Export]
    public SatelliteSystem satelliteSystem;
    [Export]
    UIPanelPoolGeneral panelGeneral;
    // UIPanelPositionFeatures panelFeatures;
    [Export]
    UIPanelPoolSupply panelSupply;
    [Export]
    UIPanelPoolTrade panelTrade;


    public override void _Ready()
    {
        Name = $"UIWindow-{satelliteSystem.Name}";

        panelGeneral.domain = satelliteSystem;
        panelSupply.domain = satelliteSystem;
        panelTrade.domain = satelliteSystem;

        // Position = (Vector2I)rp.Position;
    }
}
