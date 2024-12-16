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

    static readonly PackedScene prefab_PanelFeatures = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Position/UIPanelPositionFeatures.tscn");
    static readonly PackedScene prefab_DomainSupply = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UIPanelDomainSupply.tscn");
    static readonly PackedScene prefab_DomainTrade = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UIPanelDomainTrade.tscn");

    public override void _Ready()
    {
        GetNode<Global>("/root/Global").Connect("EFrameUI", callable: new Callable(this, "OnEFrameUI"));
        base._Ready();

        // 
        // UIPanel[] uIPanels = new UIPanel[] { };

        Name = $"UIWindow-{position.Name}";
        // Will always have general.

        panelFeatures.position = position;
        //panelSupply. = position;
        //panelTrade.domain = planet;
        OnEFrameUI();

    }

    public override void OnEFrameUpdate()
    {

        UIPanelPositionFeatures features = GetNodeOrNull<UIPanelPositionFeatures>("Features");
        if (features == null)
        {
            features = prefab_PanelFeatures.Instantiate<UIPanelPositionFeatures>();
            features.Name = "Features";
            features.position = position;
            AddChild(features);
        }

        if (typeof(Entities.IDomain).IsAssignableFrom(position.GetType()))
        {
            UIPanelDomainSupply supply = GetNodeOrNull<UIPanelDomainSupply>("Supply");
            if (supply == null)
            {
                supply = prefab_DomainSupply.Instantiate<UIPanelDomainSupply>();
                supply.Name = "Supply";
                supply.domain = ((Domain)position);
                AddChild(supply);
            }
        }
        else
        {
            UIPanelDomainSupply supply = GetNodeOrNull<UIPanelDomainSupply>("Supply");
            if (supply == null)
            {
                supply = prefab_DomainSupply.Instantiate<UIPanelDomainSupply>();
                supply.Name = "Supply";
                supply.domain = (Domain)position.Domain;
                AddChild(supply);
            }
            UIPanelDomainTrade trade = GetNodeOrNull<UIPanelDomainTrade>("Trade");
            if (trade == null)
            {
                trade = prefab_DomainSupply.Instantiate<UIPanelDomainTrade>();
                trade.Name = "Trade";
                trade.domain = ((Domain)position.Domain);
                AddChild(trade);
            }
        }


    }
}
