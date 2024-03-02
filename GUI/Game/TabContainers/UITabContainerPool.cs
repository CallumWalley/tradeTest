using Godot;
using System;
using System.Collections.Generic;

public partial class UITabContainerPool : TabContainer
{
    [Export]
    public ResourcePool rp;
    static readonly PackedScene p_tradePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelResourcePool.tscn");
    static readonly PackedScene p_astroPanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelAstro.tscn");
    // static readonly PackedScene p_featurePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UIIndustryPanel.tscn");

    UIPanelPoolGeneral panelGeneral;
    UIPanelPoolFeatures panelFeatures;

    public void Init(ResourcePool _body)
    {
        rp = _body;

        panelGeneral = GetNode<UIPanelPoolGeneral>("General");
        panelFeatures = GetNode<UIPanelPoolFeatures>("Features");

        panelFeatures.resourcePool = rp;
        panelGeneral.resourcePool = rp;
        // Position = (Vector2I)rp.Position;
    }

    public void Update()
    {
        panelGeneral.Update();
        panelFeatures.Update();
    }
}
