using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class UIPanelInstallation : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Body Body { get { return GetParent<Body>(); } }

    Installation installation;
    TabContainer tabContainer;
    BoxContainer supplyPanel;
    BoxContainer tradePanel;


    // static readonly PackedScene p_uiIndustry = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UI_Industry_Full.tscn");
    // static readonly PackedScene p_uiTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UITradeRouteFull.tscn");
    static readonly PackedScene p_uistorage = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIResourceStorage.tscn");
    // static readonly PackedScene p_resource = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn");
    static readonly PackedScene p_uirequest = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResourceRequestWithDetails.tscn");
    static readonly PackedScene p_uiDropDown = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIDropDown.tscn");
    // static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UIDropDown.tscn");
    static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");
    static readonly PackedScene prefab_UplineSelector = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Dropdowns/UIDropDownSetHead.tscn");

    public void Init(Installation _installation)
    {
        tabContainer = GetNode<VBoxContainer>("VBoxContainer").GetNode<TabContainer>("TabContainer");

        installation = _installation;

        // tabContainer.AddChild(SupplyPanel());
        // tabContainer.AddChild(TradePanel());
        // // tabContainer.AddChild(IndustryPanel());

        supplyPanel = tabContainer.GetNode<BoxContainer>("Supply");
        tradePanel = tabContainer.GetNode<BoxContainer>("Trade");


        UIListResources uiResourceProduced = new();
        UIList<Resource.IRequestable> uiResourceConsumed = new();
        UIListResources uiResourceDelta = new();
        UIList<KeyValuePair<int, Installation.StorageElement>> uiStorage = new();

        uiResourceProduced.Init(installation.produced);
        uiResourceConsumed.Init(installation.consumed, p_uirequest);
        uiResourceDelta.Init(installation.delta);
        uiStorage.Init(installation.Storage, p_uistorage);

        supplyPanel.GetNode<VBoxContainer>("Produced").AddChild(uiResourceProduced);
        supplyPanel.GetNode<VBoxContainer>("Consumed").AddChild(uiResourceConsumed); ;
        supplyPanel.GetNode<VBoxContainer>("Delta").AddChild(uiResourceDelta); ;
        supplyPanel.GetNode<VBoxContainer>("Storage").AddChild(uiStorage); ;

        UIList<TradeRoute> tradeRouteList = new();
        UIDropDownSetHead setUpline = tradePanel.GetNode<UIDropDownSetHead>("DropDown");
        setUpline.Init(installation);
        tradeRouteList.Init(installation.Trade.DownlineTraderoutes, prefab_TradeRoute);
        tradeRouteList.Vertical = true;
        tradePanel.AddChild(tradeRouteList);

        tabContainer.QueueRedraw();
    }
}
