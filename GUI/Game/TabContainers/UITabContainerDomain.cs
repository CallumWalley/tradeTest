using Godot;
using System;
using System.Collections.Generic;

public partial class UITabContainerDomain : TabContainer
{
    [Export]
    public Domain rp;
    UIPanelPoolGeneral panelGeneral;
    UIPanelDomainFeatures panelFeatures;
    UIPanelPoolSupply panelSupply;
    UIPanelPoolTrade panelTrade;


    public void Init(Domain _body)
    {
        rp = _body;
        Name = $"UIWindow-{rp.Name}";

        panelGeneral = GetNode<UIPanelPoolGeneral>("General");
        panelFeatures = GetNode<UIPanelDomainFeatures>("Features");
        panelSupply = GetNode<UIPanelPoolSupply>("Supply");
        panelTrade = GetNode<UIPanelPoolTrade>("Trade");


        panelFeatures.resourcePool = rp;
        panelGeneral.resourcePool = rp;
        panelSupply.resourcePool = rp;
        panelTrade.domain = rp;

        // Position = (Vector2I)rp.Position;
    }
}
