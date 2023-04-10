using Godot;
using System;
using System.Collections.Generic;

public class UIResourcesPanel : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public Body Body{get{return GetParent<Body>();}}

	Installation installation;
	UITransformerList UITransformerList;
	UIResourceList UIResourceList;
	UIStorageList UIStorageList;
	UIResourceList UIStockpileList;

	UITradeSourceSelector uiTradeDestinationSelector;

	static readonly PackedScene p_UITransformerList = GD.Load<PackedScene>("res://GUI/Components/UITransformerList.tscn");
	static readonly PackedScene p_UIResourceList = GD.Load<PackedScene>("res://GUI/Components/UIResourceList.tscn");
	static readonly PackedScene p_UIStorageList = GD.Load<PackedScene>("res://GUI/Components/UIStorageList.tscn");
	static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UITradeSourceSelector.tscn");

	public void Init(Installation _installation){
			// Create trade receiver component.
			installation = _installation;

			uiTradeDestinationSelector = p_uiTradeDestination.Instance<UITradeSourceSelector>();
			uiTradeDestinationSelector.Init(installation);
			AddChild(uiTradeDestinationSelector);

			UIResourceList = p_UIResourceList.Instance<UIResourceList>();
			UIResourceList.Init(installation.resourceDelta);
			AddChild(UIResourceList);

			// UIResourceList2 = p_UIResourceList.Instance<UIResourceList>();
			// UIResourceList2.Init(installation.resourceDeltaConsumed);
			// AddChild(UIResourceList2);

			// UIResourceList3 = p_UIResourceList.Instance<UIResourceList>();
			// UIResourceList3.Init(installation.resourceDelta);
			// AddChild(UIResourceList3);
			// UIStockpileList = p_UIResourceList.Instance<UIResourceList>();
			// UIStockpileList.Init(installation.resourceStockpile);
			// AddChild(UIStockpileList);

			UIStorageList = p_UIStorageList.Instance<UIStorageList>();
			UIStorageList.Init(installation);
			AddChild(UIStorageList);



			UITransformerList = p_UITransformerList.Instance<UITransformerList>();
			UITransformerList.Init(installation);
			AddChild(UITransformerList);
			UITransformerList.Visible=true;
	}


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
