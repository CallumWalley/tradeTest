using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UIPanelPosition : UIPanel
{
    public Entities.IPosition position;

    [Export]
    UIPanelPositionFeatures panelFeatures;
    [Export]
    UIPanelDomainSupply panelSupply;
    [Export]
    UIPanelDomainTrade panelTrade;

    [Export]
    UIOrbitalDescription orbitalDescription;
    public GridContainer gridContainer;
    public TabContainer tabContainer;

    public UIRename name;

    static readonly PackedScene prefab_PanelFeatures = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Position/UIPanelPositionFeatures.tscn");
    static readonly PackedScene prefab_DomainSupply = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UIPanelDomainSupply.tscn");
    static readonly PackedScene prefab_DomainTrade = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UIPanelDomainTrade.tscn");

    public override void _Ready()
    {
        base._Ready();
        name = GetNode<UIRename>("VBoxContainer/GridContainer/HBoxContainer/VBoxContainer/HBoxContainer/UiRename");
        name.entity = position;
        orbitalDescription.orbital = position;
        tabContainer = GetNode<TabContainer>("VBoxContainer/GridContainer/TabContainer");
        Name = $"UIWindow-{position.Name}";
        OnEFrameUI();

    }

    public override void OnEFrameUpdate()
    {
        UIPanelPositionFeatures features = tabContainer.GetNodeOrNull<UIPanelPositionFeatures>("Features");
        if (features == null)
        {
            features = prefab_PanelFeatures.Instantiate<UIPanelPositionFeatures>();
            features.Name = "Features";
            features.position = position;
            tabContainer.AddChild(features);
        }
        if (typeof(Entities.IDomain).IsAssignableFrom(position.GetType()))
        {
            UIPanelDomainSupply supply = tabContainer.GetNodeOrNull<UIPanelDomainSupply>("Supply");
            if (supply == null)
            {
                supply = prefab_DomainSupply.Instantiate<UIPanelDomainSupply>();
                supply.Name = "Supply";
                supply.domain = ((Domain)position);
                tabContainer.AddChild(supply);
            }
        }
        else
        {
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
                trade = prefab_DomainSupply.Instantiate<UIPanelDomainTrade>();
                trade.Name = "Trade";
                trade.domain = ((Domain)position.Domain);
                tabContainer.AddChild(trade);
            }
        }
    }
}
