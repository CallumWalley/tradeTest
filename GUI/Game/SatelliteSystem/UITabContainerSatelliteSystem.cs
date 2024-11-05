using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UITabContainerSatelliteSystem : TabContainer
{
    [Export]
    public SatelliteSystem satelliteSystem;
    [Export]
    UIPanelPositionGeneral panelGeneral;
    // UIPanelPositionFeatures panelFeatures;
    [Export]
    UIPanelDomainSupply panelSupply;
    [Export]
    UIPanelDomainTrade panelTrade;


    public override void _Ready()
    {
        Name = $"UIWindow-{satelliteSystem.Name}";

        panelGeneral.position = satelliteSystem;
        panelSupply.domain = satelliteSystem;
        panelTrade.domain = satelliteSystem;

        // Position = (Vector2I)rp.Position;
    }
}
