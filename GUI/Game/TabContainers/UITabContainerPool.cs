using Godot;
using System;
using System.Collections.Generic;

public partial class UITabContainerPool : TabContainer
{
    [Export]
    public Domain rp;
    UIPanelPoolGeneral panelGeneral;
    UIPanelPoolFeatures panelFeatures;
    UIPanelPoolSupply panelSupply;
    UIPanelPoolTrade panelTrade;


    public void Init(Domain _body)
    {
        rp = _body;
        Name = $"UIWindow-{rp.Name}";

        panelGeneral = GetNode<UIPanelPoolGeneral>("General");
        panelFeatures = GetNode<UIPanelPoolFeatures>("Features");
        panelSupply = GetNode<UIPanelPoolSupply>("Supply");
        panelTrade = GetNode<UIPanelPoolTrade>("Trade");


        panelFeatures.resourcePool = rp;
        panelGeneral.resourcePool = rp;
        panelSupply.resourcePool = rp;
        panelTrade.resourcePool = rp;

        // Position = (Vector2I)rp.Position;
    }
}
