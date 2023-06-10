using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class UIInstallation : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Body Body { get { return GetParent<Body>(); } }

    Installation installation;
    TabContainer tabContainer;
    static readonly PackedScene p_uiIndustry = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIIndustry.tscn");
    static readonly PackedScene p_uiTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeRoute.tscn");
    static readonly PackedScene p_storage = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIStorage.tscn");
    // static readonly PackedScene p_uiTransformer = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIIndustry.tscn");

    static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UITradeSourceSelector.tscn");

    public void Init(Installation _installation)
    {
        tabContainer = new TabContainer();
        installation = _installation;

        tabContainer.AddChild(SupplyPanel());
        tabContainer.AddChild(TradePanel());
        tabContainer.AddChild(IndustryPanel());

        AddChild(tabContainer);
        tabContainer.QueueRedraw();

        VBoxContainer SupplyPanel()
        {
            VBoxContainer supplyPanel = new();
            supplyPanel.Name = "Supply";

            Label titleProduced = new();
            titleProduced.Text = "Produced";
            supplyPanel.AddChild(titleProduced);

            UIResourceList uiResourceProduced = new();
            uiResourceProduced.Init(installation.resourceProduced);
            supplyPanel.AddChild(uiResourceProduced);

            Label titleConsumed = new();
            titleConsumed.Text = "Consumed";
            supplyPanel.AddChild(titleConsumed);

            UIResourceList uiResourceConsumed = new();
            uiResourceConsumed.Init(installation.resourceConsumed);
            supplyPanel.AddChild(uiResourceConsumed);

            Label titleDelta = new();
            titleDelta.Text = "Delta";
            supplyPanel.AddChild(titleDelta);

            UIResourceList uiResourceDelta = new();
            uiResourceDelta.Init(installation.resourceDelta);
            supplyPanel.AddChild(uiResourceDelta);

            Label titleStored = new();
            titleStored.Text = "Stored";
            supplyPanel.AddChild(titleStored);

            UIList uiStorage = new();
            uiStorage.Init(installation.resourceStorage, p_storage);
            supplyPanel.AddChild(uiStorage);

            return supplyPanel;
        }

        VBoxContainer TradePanel()
        {
            VBoxContainer tradePanel = new();
            tradePanel.Name = "Trade";

            UITradeSourceSelector uiTradeDestinationSelector = p_uiTradeDestination.Instantiate<UITradeSourceSelector>();
            uiTradeDestinationSelector.Init(installation);
            tradePanel.AddChild(uiTradeDestinationSelector);

            UIList uiTradeListDownline = new();
            uiTradeListDownline.Vertical = true;
            uiTradeListDownline.Init(installation.DownlineTraderoutes, p_uiTradeRoute);
            tradePanel.AddChild(uiTradeListDownline);

            return tradePanel;
        }

        VBoxContainer IndustryPanel()
        {
            VBoxContainer uiIndustryPanel = new();
            uiIndustryPanel.Name = "Industry";

            UIList uiIndustryList = new();
            uiIndustryList.Init(installation.Industries, p_uiIndustry);
            uiIndustryList.Vertical = true;

            uiIndustryPanel.AddChild(uiIndustryList);

            return uiIndustryPanel;
        }


    }
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
