using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class UIPanelResourcePool : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Body Body { get { return GetParent<Body>(); } }

    ResourcePool ResourcePool;
    TabContainer tabContainer;
    BoxContainer supplyPanel;
    BoxContainer tradePanel;
    Label tradePanelNetwork;


    // static readonly PackedScene p_uiIndustry = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UI_Industry_Full.tscn");
    // static readonly PackedScene p_uiTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UITradeRouteFull.tscn");
    static readonly PackedScene p_uistorage = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIResourceStorage.tscn");
    static readonly PackedScene p_uirequest = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn");
    static readonly PackedScene p_uiDropDown = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIDropDown.tscn");
    // static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UIDropDown.tscn");
    static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");
    static readonly PackedScene prefab_UplineSelector = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Dropdowns/UIDropDownSetHead.tscn");
    UIPanelLedger panelLedger;
    UIList<TradeRoute> tradeRouteList;
    public void Init(ResourcePool _ResourcePool)
    {
        tabContainer = GetNode<VBoxContainer>("VBoxContainer").GetNode<TabContainer>("TabContainer");
        ResourcePool = _ResourcePool;

        // tabContainer.AddChild(SupplyPanel());
        // tabContainer.AddChild(TradePanel());
        // // tabContainer.AddChild(IndustryPanel());

        supplyPanel = tabContainer.GetNode<BoxContainer>("Supply");
        tradePanel = tabContainer.GetNode<BoxContainer>("Trade");
        tradePanelNetwork = tradePanel.GetNode<Label>("Network/Label");

        panelLedger = new();
        panelLedger.Ledger = ResourcePool.Ledger;
        supplyPanel.AddChild(panelLedger);

        // Lists.UIListResources uiResourceProduced = new();
        // Lists.UIListRequestable uiResourceConsumed = new();
        // Lists.UIListResources uiResourceDelta = new();
        // Lists.UIListResources uiResourceTraded = new();

        // UIList<KeyValuePair<int, ResourcePool.StorageElement>> uiStorage = new();

        // uiResourceProduced.Init(ResourcePool.produced);
        // uiResourceConsumed.Init(ResourcePool.consumed);
        // uiResourceConsumed.ShowBreakdown = true;
        // uiResourceProduced.ShowBreakdown = true;

        // uiResourceTraded.Init(ResourcePool.traded);

        // uiResourceDelta.Init(ResourcePool.delta);
        // uiStorage.Init(ResourcePool.Storage, p_uistorage);

        // supplyPanel.GetNode<VBoxContainer>("Produced").AddChild(uiResourceProduced);
        // supplyPanel.GetNode<VBoxContainer>("Consumed").AddChild(uiResourceConsumed);
        // supplyPanel.GetNode<VBoxContainer>("Traded").AddChild(uiResourceTraded);
        // supplyPanel.GetNode<VBoxContainer>("Delta").AddChild(uiResourceDelta);
        // supplyPanel.GetNode<VBoxContainer>("Storage").AddChild(uiStorage);

        tradeRouteList = new();
        UIDropDownSetHead setUpline = tradePanel.GetNode<UIDropDownSetHead>("DropDown");
        setUpline.Init(ResourcePool);
        tradeRouteList.Init(ResourcePool.Trade.DownlineTraderoutes, prefab_TradeRoute);
        tradeRouteList.Vertical = true;
        tradePanel.AddChild(tradeRouteList);

        tabContainer.QueueRedraw();
    }



    public override void _Draw()
    {
        base._Draw();
        if (ResourcePool.Order > 0)
        {
            tradePanelNetwork.Text = string.Format("{0} order member of {1}", ResourcePool.Order, ResourcePool.Network);
            //tradePanelNetwork.Visible = true;
        }
        else
        {
            tradePanelNetwork.Text = string.Format("No trade network connected.", ResourcePool.Order, ResourcePool.Network);
            //tradePanelNetwork.Visible = false;
        }
        bool showTrade = (ResourcePool.Trade.UplineTraderoute != null || ResourcePool.Trade.DownlineTraderoutes.Count > 0);
        GetNode<Label>("VBoxContainer/TabContainer/Supply/VBoxContainer/Trade").Visible = showTrade;
        panelLedger.ShowTrade = showTrade;
        panelLedger.Update();
        tradeRouteList.Update();
    }
}
