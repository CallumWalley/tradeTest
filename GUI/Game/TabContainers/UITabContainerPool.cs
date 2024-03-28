using Godot;
using System;
using System.Collections.Generic;

public partial class UITabContainerPool : UITabContainer
{
    [Export]
    public ResourcePool rp;
    static readonly PackedScene p_tradePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelResourcePool.tscn");
    static readonly PackedScene p_astroPanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelAstro.tscn");
    // static readonly PackedScene p_featurePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UIIndustryPanel.tscn");

    UIPanelPoolGeneral panelGeneral;
    UIPanelPoolFeatures panelFeatures;
    UIPanelPoolSupply panelSupply;


    public void Init(ResourcePool _body)
    {
        rp = _body;

        panelGeneral = GetNode<UIPanelPoolGeneral>("General");
        panelFeatures = GetNode<UIPanelPoolFeatures>("Features");
        panelSupply = GetNode<UIPanelPoolSupply>("Supply");


        panelFeatures.resourcePool = rp;
        panelGeneral.resourcePool = rp;
        panelSupply.resourcePool = rp;
        // Position = (Vector2I)rp.Position;
    }
}
