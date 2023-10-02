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
    // static readonly PackedScene p_uiIndustry = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UI_Industry_Full.tscn");
    // static readonly PackedScene p_uiTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UITradeRouteFull.tscn");
    static readonly PackedScene p_uistorage = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIResourceStorage.tscn");
    // static readonly PackedScene p_resource = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Listables/UIResource.tscn");
    static readonly PackedScene p_uirequest = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Listables/UIResourceRequestWithDetails.tscn");
    static readonly PackedScene p_uiDropDown = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIDropDown.tscn");
    // static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UIDropDown.tscn");
    static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Listables/TradeRoute/UITradeRouteFull.tscn");
    static readonly PackedScene prefab_UplineSelector = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Dropdowns/UIDropDownSetUpline.tscn");

    public void Init(Installation _installation)
    {
        tabContainer = GetNode<VBoxContainer>("VBoxContainer").GetNode<TabContainer>("TabContainer");
        tabContainer.ClipChildren = 0;
        tabContainer.ClipTabs = false;

        installation = _installation;

        tabContainer.AddChild(SupplyPanel());
        tabContainer.AddChild(TradePanel());
        // tabContainer.AddChild(IndustryPanel());

        tabContainer.QueueRedraw();

        VBoxContainer SupplyPanel()
        {
            VBoxContainer supplyPanel = new();
            supplyPanel.Name = "Supply";

            Label titleProduced = new();
            titleProduced.Text = "Produced";
            supplyPanel.AddChild(titleProduced);

            UIResourceList uiResourceProduced = new();
            uiResourceProduced.Init(installation.produced);
            supplyPanel.AddChild(uiResourceProduced);

            Label titleConsumed = new();
            titleConsumed.Text = "Consumed";
            supplyPanel.AddChild(titleConsumed);

            UIList<Resource.IRequestable> uiResourceConsumed = new();
            uiResourceConsumed.Init(installation.consumed, p_uirequest);
            supplyPanel.AddChild(uiResourceConsumed);

            Label titleDelta = new();
            titleDelta.Text = "Delta";
            supplyPanel.AddChild(titleDelta);

            UIResourceList uiResourceDelta = new();
            uiResourceDelta.Init(installation.delta);
            supplyPanel.AddChild(uiResourceDelta);

            Label titleStored = new();
            titleStored.Text = "Stored";
            supplyPanel.AddChild(titleStored);

            UIList<KeyValuePair<int, Installation.StorageElement>> uiStorage = new();
            uiStorage.Init(installation.Storage, p_uistorage);
            supplyPanel.AddChild(uiStorage);

            return supplyPanel;
        }

        VBoxContainer TradePanel()
        {
            VBoxContainer tradePanel = new();
            UIList<TradeRoute> tradeRouteList;
            UIDropDown setUpline;


            tradePanel.Name = "Trade";

            tradeRouteList = new();
            setUpline = prefab_UplineSelector.Instantiate<UIDropDownSetUpline>();

            tradeRouteList.Init(installation.DownlineTraderoutes, prefab_TradeRoute);
            tradePanel.AddChild(setUpline);
            tradePanel.AddChild(tradeRouteList);
            // UIDropDown = p_uiDropDown.Instantiate<UIDropDown>();

            // tradePanel.AddChild()

            return tradePanel;
        }

        // VBoxContainer TradePanel()
        // {
        //     VBoxContainer tradePanel = new();
        //     tradePanel.Name = "Trade";

        //     //TextureRect netIncoming = new();
        //     //netIncomingValue
        //     //netIncoming.Texture = "res://assets/icons/trade_value.dds";
        //     //UIResource netOutgoing = p_resource.Instantiate<UIResource>();


        //     HBoxContainer tradeBalance = new();
        //     // tradeBalance.AddChild(netIncoming);
        //     // tradeBalance.AddChild(netOutgoing);

        //     tradePanel.AddChild(tradeBalance);

        //     UIDropDown uiTradeDestinationSelector = p_uiTradeDestination.Instantiate<UIDropDown>();
        //     uiTradeDestinationSelector.Init(installation);
        //     tradePanel.AddChild(uiTradeDestinationSelector);

        //     tradePanel.AddChild(new HSeparator());

        //     UIList<TradeRoute> uiTradeListDownline = new();
        //     uiTradeListDownline.Vertical = true;
        //     uiTradeListDownline.Init(installation.DownlineTraderoutes, p_uiTradeRoute);
        //     tradePanel.AddChild(uiTradeListDownline);

        //     return tradePanel;
        // }

        // VBoxContainer IndustryPanel()
        // {
        //     VBoxContainer uiIndustryPanel = new();
        //     uiIndustryPanel.Name = "Industry";

        //     UIList<Industry> uiIndustryList = new();
        //     uiIndustryList.Init(installation.Industries, p_uiIndustry);
        //     uiIndustryList.Vertical = true;

        //     uiIndustryPanel.AddChild(uiIndustryList);

        //     return uiIndustryPanel;
        // }
    }

    // public override void _Ready()
    // {
    //     // Redraw every eframe

    //     GetNode<Global>("/root/Global").Connect("EFrame", new Callable(this, "QueueRedrawWrap"));
    // }


    // TODO: ask noel for heeeeelppppppp
    // IEnumerable<System.Object> WhyAreYouLikeThis(Node aaaaaaaa)
    // {
    //     return aaaaaaaa.GetChildren();
    //     foreach (System.Object aaaaaaaaaaaa in aaaaaaaa)
    //     {
    //         yield return aaaaaaaaaaaa;
    //     }
    // }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
