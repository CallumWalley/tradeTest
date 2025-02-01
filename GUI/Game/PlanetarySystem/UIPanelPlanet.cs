using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UIPanelPlanet : UIPanelPosition
{

    [Export]
    UIPanelDomainTrade panelTrade;

    static readonly PackedScene prefab_PanelFeatures = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Position/UIPanelPositionFeatures.tscn");
    static readonly PackedScene prefab_DomainSupply = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UIPanelDomainSupply.tscn");
    static readonly PackedScene prefab_DomainTrade = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UIPanelDomainTrade.tscn");


    public override void OnEFrameUpdate()
    {
        base.OnEFrameUpdate();

        UIPanelDomainSupply supply = tabContainer.GetNodeOrNull<UIPanelDomainSupply>("Supply");
        if (supply == null)
        {
            supply = prefab_DomainSupply.Instantiate<UIPanelDomainSupply>();
            supply.Name = "Supply";
            supply.domain = (Domain)position.Domain;
            tabContainer.AddChild(supply);
        }
        UIPanelDomainTrade trade = tabContainer.GetNodeOrNull<UIPanelDomainTrade>("Trade");
        if (trade == null)
        {
            trade = prefab_DomainTrade.Instantiate<UIPanelDomainTrade>();
            trade.Name = "Trade";
            trade.domain = ((Domain)position.Domain);
            tabContainer.AddChild(trade);
        }
    }
}
