using Godot;
using System;
using System.Collections.Generic;

public class UIResourcesPanel : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Body Body { get { return GetParent<Body>(); } }

    Installation installation;
    UIList uiTransformerList;
    UIResourceList uiResourceList;
    UIStorageList uiStorageList;
    UIList uiStockpileList;

    UITradeSourceSelector uiTradeDestinationSelector;
    static readonly PackedScene p_transformer = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITransformer.tscn");
    static readonly PackedScene p_vlist = GD.Load<PackedScene>("res://GUI/Components/UIListVBox.tscn");
    static readonly PackedScene p_hlist = GD.Load<PackedScene>("res://GUI/Components/UIListHBox.tscn");
    static readonly PackedScene p_UIStorageList = GD.Load<PackedScene>("res://GUI/Components/UIStorageList.tscn");
    static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UITradeSourceSelector.tscn");

    public void Init(Installation _installation)
    {
        // Create trade receiver component.
        installation = _installation;

        uiTradeDestinationSelector = p_uiTradeDestination.Instance<UITradeSourceSelector>();
        uiTradeDestinationSelector.Init(installation);
        AddChild(uiTradeDestinationSelector);

        uiResourceList = p_hlist.Instance<UIResourceList>();
        uiResourceList.Init(installation.resourceDelta.GetStandard());
        AddChild(uiResourceList);

        // UIResourceList2 = p_UIResourceList.Instance<UIResourceList>();
        // UIResourceList2.Init(installation.resourceDeltaConsumed);
        // AddChild(UIResourceList2);

        // UIResourceList3 = p_UIResourceList.Instance<UIResourceList>();
        // UIResourceList3.Init(installation.resourceDelta);
        // AddChild(UIResourceList3);
        // UIStockpileList = p_UIResourceList.Instance<UIResourceList>();
        // UIStockpileList.Init(installation.resourceStockpile);
        // AddChild(UIStockpileList);

        uiStorageList = p_UIStorageList.Instance<UIStorageList>();
        uiStorageList.Init(installation);
        AddChild(uiStorageList);



        uiTransformerList = p_vlist.Instance<UIList>();
        uiTransformerList.Init(WhyAreYouLikeThis(installation.GetChildren()), p_transformer);
        AddChild(uiTransformerList);
        uiTransformerList.Visible = true;
    }
    // TODO: ask noel for heeeeelppppppp
    IEnumerable<System.Object> WhyAreYouLikeThis(Godot.Collections.Array aaaaaaaa)
    {
        foreach (System.Object aaaaaaaaaaaa in aaaaaaaa)
        {
            yield return aaaaaaaaaaaa;
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
