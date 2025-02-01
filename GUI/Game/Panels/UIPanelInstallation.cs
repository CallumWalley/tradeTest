using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
namespace Game;

public partial class UIPanelDomain : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Body Body { get { return GetParent<Body>(); } }

    Domain Domain;
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
    static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/TradeRoute/UITradeRouteFull.tscn");
    static readonly PackedScene prefab_UplineSelector = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Dropdowns/UIDropDownSetHead.tscn");
    UIPanelLedger panelLedger;
    UIList<TradeRoute> tradeRouteList;
    public void Init(Domain _Domain)
    {
        tabContainer = GetNode<VBoxContainer>("VBoxContainer").GetNode<TabContainer>("TabContainer");
        Domain = _Domain;

        // tabContainer.AddChild(SupplyPanel());
        // tabContainer.AddChild(TradePanel());
        // // tabContainer.AddChild(IndustryPanel());

        supplyPanel = tabContainer.GetNode<BoxContainer>("Supply");
        tradePanel = tabContainer.GetNode<BoxContainer>("Trade");
        tradePanelNetwork = tradePanel.GetNode<Label>("Network/Label");

        panelLedger = new();
        panelLedger.Ledger = Domain.Ledger;
        supplyPanel.AddChild(panelLedger);

        // Lists.UIListResources uiResourceProduced = new();
        // Lists.UIListRequestable uiResourceConsumed = new();
        // Lists.UIListResources uiResourceDelta = new();
        // Lists.UIListResources uiResourceTraded = new();

        // UIList<KeyValuePair<int, Domain.StorageElement>> uiStorage = new();

        // uiResourceProduced.Init(Domain.produced);
        // uiResourceConsumed.Init(Domain.consumed);
        // uiResourceConsumed.ShowBreakdown = true;
        // uiResourceProduced.ShowBreakdown = true;

        // uiResourceTraded.Init(Domain.traded);

        // uiResourceDelta.Init(Domain.delta);
        // uiStorage.Init(Domain.Storage, p_uistorage);

        // supplyPanel.GetNode<VBoxContainer>("Produced").AddChild(uiResourceProduced);
        // supplyPanel.GetNode<VBoxContainer>("Consumed").AddChild(uiResourceConsumed);
        // supplyPanel.GetNode<VBoxContainer>("Traded").AddChild(uiResourceTraded);
        // supplyPanel.GetNode<VBoxContainer>("Delta").AddChild(uiResourceDelta);
        // supplyPanel.GetNode<VBoxContainer>("Storage").AddChild(uiStorage);

        tradeRouteList = new();
        UIDropDownSetHead setUpline = tradePanel.GetNode<UIDropDownSetHead>("DropDown");
        setUpline.Domain = Domain;
        tradeRouteList.Init(Domain.DownlineTraderoutes, prefab_TradeRoute);
        tradeRouteList.Vertical = true;
        tradePanel.AddChild(tradeRouteList);

        tabContainer.QueueRedraw();
    }



    public override void _Draw()
    {
        base._Draw();
        if (Domain.Order > 0)
        {
            tradePanelNetwork.Text = string.Format("{0} order member of {1}", Domain.Order, Domain.Network);
            //tradePanelNetwork.Visible = true;
        }
        else
        {
            tradePanelNetwork.Text = string.Format("No trade network connected.", Domain.Order, Domain.Network);
            //tradePanelNetwork.Visible = false;
        }
        bool showTrade = (Domain.UplineTraderoute != null || Domain.DownlineTraderoutes.Count > 0);
        GetNode<Label>("VBoxContainer/TabContainer/Supply/VBoxContainer/Trade").Visible = showTrade;
        panelLedger.ShowTrade = showTrade;
        panelLedger.Update();
        tradeRouteList.Update();
    }
}
