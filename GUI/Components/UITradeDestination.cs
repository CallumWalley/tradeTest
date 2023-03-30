using Godot;
using System;
using System.Linq;
using System.Collections;

public class UITradeDestination : Control
{   
	public PlayerTradeReciever globalTrade;
	static readonly Texture freighterIcon = GD.Load<Texture>("res://assets/icons/freighter.png");
	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIResource.tscn");

	ResourcePool resourcePool;

	Body body;

	// Index of trade route destingation if exists already;
	[Export]
	public int indexOfExisting=0;
	public bool receiver = false;

	// Children members
	OptionButton tradeDestSelectorButton;
	Godot.Collections.Array<ResourcePool> validDestinations;
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

		//OptionButton tradeDestSelectorDropdown = GetNode("Panel/TradeDestination/Dropdown");
		// Connect("mouse_entered", tradeDestSelector, "Focus");
		// Connect("mouse_exited", tradeDestSelector, "UnFocus");
		tradeDestSelectorButton.Connect("item_selected", this, "DestinationSelected");
	}

	public override void _Draw(){
		if (resourcePool == null){return;}
		tradeDestSelectorButton.Clear();
		// Todo better structure to avoid this check.
		tradeDestSelectorButton.AddIconItem(freighterIcon, " 0 - No Route Assigned", 0);
		int indexId = 1; //Start at 1 to allow for 'none' in []
		indexOfExisting = 0;
		
		//Filter to only valid options
		validDestinations = new Godot.Collections.Array<ResourcePool>(GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").List().Where(x => ((x != resourcePool))));
	
		foreach (ResourcePool rp in validDestinations){
			// Self not valid source
			float dist = body.Position.DistanceTo(rp.Position);
			float freighterKTons = GetNode<PlayerTech>("/root/Global/Player/Tech").GetFreighterTons(resourcePool.shipWeight, dist);
			tradeDestSelectorButton.AddIconItem(freighterIcon, String.Format(" {0:F1} - {1}",freighterKTons, rp.GetParent<Body>().Name), indexId);		
			// set index of existing route (if any)
			if (resourcePool.uplineTraderoute != null && resourcePool.uplineTraderoute.poolSource == rp){
				// If trade route was disestablished elsewhere.
				// if (! resourcePool.uplineTraderoute.poolSource.IsAParentOf(resourcePool.uplineTraderoute.transformerSource)){
				// 	indexOfExisting = 0;
				// }else{
				indexOfExisting = indexId;
				// }
				tradeDestSelectorButton.Selected = indexOfExisting;
			}
			indexId ++;
		}
	}

	public void DestinationSelected(int value){
		GD.Print($"Existing trade route index {indexOfExisting}, selected {value}");
		// If this is true, no change has been made.
		if (value != indexOfExisting){
			// If existing non-null, remove it.
			if (indexOfExisting != 0){
				GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(resourcePool.uplineTraderoute);
				resourcePool.uplineTraderoute = null;
			}
			// If selection non-null, create new trade route.
			if ( value != 0 ){
				GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").RegisterTradeRoute(resourcePool, validDestinations[value - 1]);
			}
		}
		tradeDestSelectorButton.Selected = indexOfExisting;
		Update();
	}
}
