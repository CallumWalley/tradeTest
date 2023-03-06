using Godot;
using System;

public class UITradeSource : Control
{   
	[Export]
	public TradeSource tradeSource;
	public GlobalTradeReciever globalTrade;
	static readonly Texture freighterIcon = GD.Load<Texture>("res://assets/icons/freighter.png");
	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/Components/UIResource.tscn");

	// Index of trade route destingation if exists already;
	[Export]
	public int indexOfExisting=0;
	public bool receiver = false;

	// Children members
	OptionButton tradeDestSelectorButton;


	public override void _Ready()
	{
		Control tradeDestSelector = GetNode<Control>("TradeDestination");
		tradeDestSelectorButton = (OptionButton)GetNode("TradeDestination/Dropdown");
		globalTrade = GetNode<GlobalTradeReciever>("/root/Global/Trade");

		//OptionButton tradeDestSelectorDropdown = GetNode("Panel/TradeDestination/Dropdown");
		// Connect("mouse_entered", tradeDestSelector, "Focus");
		// Connect("mouse_exited", tradeDestSelector, "UnFocus");
		//tradeDestSelectorButton.Connect("mouse_exited", this, "UnFocus");
	}

	public void Init(TradeSource _tradeSource){
		tradeSource=_tradeSource;
		foreach (Resource r in tradeSource.resourcePool.GetChildren()){
			UIResource ui = resourceIcon.Instance<UIResource>();
			ui.Init(r);
			GetNode("ResourcePool").AddChild(ui);
		}
	}
	// public override void Focus(){
	// 	base.Focus();
	// 	UpdateNumbers();
	// }
	public override void _Draw(){
		//if (tradeSource == null ){return;}
		//Clear();
		tradeDestSelectorButton.Clear();
		// Todo better structure to avoid this check.
		tradeDestSelectorButton.AddIconItem(freighterIcon, "[ 0 / 0 ] - No Route Assigned", 0);
		int indexId = 1;
	
		foreach (TradeReceiver tr in globalTrade.List()){
			float dist = tradeSource.Position.DistanceTo(tr.Position);
			float freighterKTons = GetNode<GlobalTech>("/root/Global/Tech").GetFreighterTons(tradeSource.shipWeight, dist);
			tradeDestSelectorButton.AddIconItem(freighterIcon, String.Format("[ {0:F1}  / {1:F1} ] - {2}",freighterKTons, tr.resourcePool.GetType(901).Sum, tr.name), indexId);		
			if (tradeSource.tradeRoute != null && tradeSource.tradeRoute.tradeReceiver == tr){
				tradeDestSelectorButton.Selected = indexId;
				indexOfExisting = indexId;
			}
			indexId ++;
		}
	}

	// TODO: show route on hover.

	public void DestinationSelected(int value){
		GD.Print($"Existing trade route index {indexOfExisting}, selected {value}");

		GD.Print(globalTrade.List());
		// If this is true, no change has been made.
		if (value != indexOfExisting){
			// If existing non-null, remove it.
			if (indexOfExisting != 0){
				globalTrade.List()[indexOfExisting-1].DeregisterTradeRoute(tradeSource.tradeRoute);
				tradeSource.tradeRoute = null;
			}
			// If selection non-null, create new trade route.
			if ( value != 0 ){
				globalTrade.List()[value-1].RegisterTradeRoute(tradeSource);
			}
		}
		tradeDestSelectorButton.Selected = indexOfExisting;

	}
}
