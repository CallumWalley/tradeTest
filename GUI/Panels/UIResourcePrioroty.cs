using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class UIResourcePrioroty : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Body Body { get { return GetParent<Body>(); } }

    Installation installation;
    UIList uiIndustryList;
    UIResourceList uiResourceList;
    UIList uiStorageList;

    UITradeSourceSelector uiTradeDestinationSelector;
    static readonly PackedScene p_Industry = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UI_Industry_Full.tscn");
    static readonly PackedScene p_storage = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIStorage.tscn");
    static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UITradeSourceSelector.tscn");

    public void Init(Installation _installation)
    {
        // Create trade receiver component.
        installation = _installation;

        uiTradeDestinationSelector = p_uiTradeDestination.Instantiate<UITradeSourceSelector>();
        uiTradeDestinationSelector.Init(installation);
        AddChild(uiTradeDestinationSelector);


        uiResourceList = new UIResourceList();//new UIResourceList();
        uiResourceList.Init(installation.resourceDelta.Standard);
        AddChild(uiResourceList);

        // UResource.RList2 = p_UResource.RList.Instantiate<UResource.RList>();
        // UResource.RList2.Init(installation.resourceDeltaConsumed);
        // AddChild(UResource.RList2);

        // UResource.RList3 = p_UResource.RList.Instantiate<UResource.RList>();
        // UResource.RList3.Init(installation.resourceDelta);
        // AddChild(UResource.RList3);
        // UIStockpileList = p_UResource.RList.Instantiate<UResource.RList>();
        // UIStockpileList.Init(installation.resourceStockpile);
        // AddChild(UIStockpileList);

        uiIndustryList = new UIList();//p_vlist.Instantiate<UIContainers.UIListChildren>();
        uiIndustryList.Init(installation.Industries, p_Industry);
        uiIndustryList.Vertical = true;
        //uiIndustryList

        AddChild(uiIndustryList);
        uiIndustryList.Visible = true;
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
