using Godot;
using System;
using System.Collections.Generic;
namespace Game;

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


        panelFeatures.domain = rp;
        panelGeneral.resourcePool = rp;
        panelSupply.resourcePool = rp;
        panelTrade.domain = rp;

        // Position = (Vector2I)rp.Position;
    }
}
