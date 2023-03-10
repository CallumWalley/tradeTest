using Godot;
using System;

public class UITradeDestination : Control
{   
	[Export]
	public GlobalTradeReciever globalTrade;
	static readonly Texture freighterIcon = GD.Load<Texture>("res://assets/icons/freighter.png");
	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIResource.tscn");

	ResourcePool resourcePool;

	Body body;

	// Index of trade route destingation if exists already;
	[Export]
	public int indexOfExisting=0;
	public bool receiver = false;

	// Children members
	OptionButton tradeDestSelectorButton;

	public void Init(Body _body, ResourcePool _resourcePool){
		resourcePool=_resourcePool;
		body = _body;
		// foreach (Resource r in tradeSource.resourcePool.GetStandard()){
		// 	UIResource ui = resourceIcon.Instance<UIResource>();
		// 	ui.Init(r);
		// 	//ui.showDetails = true;
		// 	GetNode("ResourcePool").AddChild(ui);
		// }
	}

	public override void _Ready()
	{
		tradeDestSelectorButton = (OptionButton)GetNode("Dropdown");
		globalTrade = GetNode<GlobalTradeReciever>("/root/Global/Trade");

		//OptionButton tradeDestSelectorDropdown = GetNode("Panel/TradeDestination/Dropdown");
		// Connect("mouse_entered", tradeDestSelector, "Focus");
		// Connect("mouse_exited", tradeDestSelector, "UnFocus");
		tradeDestSelectorButton.Connect("item_selected", this, "DestinationSelected");
	}


	// public override void Focus(){
	// 	base.Focus();
	// 	UpdateNumbers();
	// }
	public override void _Draw(){
		if (resourcePool == null){return;}
		tradeDestSelectorButton.Clear();
		// Todo better structure to avoid this check.
		tradeDestSelectorButton.AddIconItem(freighterIcon, "[ 0 / 0 ] - No Route Assigned", 0);
		int indexId = 0;
		indexOfExisting = 0;
		
		foreach (TradeReceiver tr in globalTrade.List()){
			indexId ++;
			// Self not valid source
			if ((body.tradeReceiver != null) && (body.tradeReceiver == tr )){continue;}
			float dist = body.Position.DistanceTo(tr.Position);
			float freighterKTons = GetNode<GlobalTech>("/root/Global/Tech").GetFreighterTons(resourcePool.shipWeight, dist);
			tradeDestSelectorButton.AddIconItem(freighterIcon, String.Format("[ {0:F1}  / {1:F1} ] - {2}",freighterKTons, resourcePool.GetType(901).Sum, tr.name), indexId);		
			
			// If pool has trade route, and this is trade receiver.
			if (resourcePool.tradeRoute != null && resourcePool.tradeRoute.tradeReceiver == tr){
				tradeDestSelectorButton.Selected = indexId;
				indexOfExisting = indexId;
			}
			
		}
	}

	// TODO: show route on hover.

	public void DestinationSelected(int value){
		GD.Print($"Existing trade route index {indexOfExisting}, selected {value}");
		// If this is true, no change has been made.
		if (value != indexOfExisting){
			// If existing non-null, remove it.
			if (indexOfExisting != 0){
				globalTrade.List()[indexOfExisting-1].DeregisterTradeRoute(resourcePool.tradeRoute);
				resourcePool.tradeRoute = null;
			}
			// If selection non-null, create new trade route.
			if ( value != 0 ){
				globalTrade.List()[value-1].RegisterTradeRoute(body, resourcePool);
			}
		}
		tradeDestSelectorButton.Selected = indexOfExisting;
		Update();
	}
}
