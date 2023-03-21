using Godot;
using System;
using System.Collections.Generic;

public class UITradeRoute : Control
{
	public TradeRoute tradeRoute;

	public ResourcePool resourcePool;
	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIResource.tscn");

	public static PlayerTradeRoutes playerTradeRoutes;

	TextureButton moveUpButton;
	TextureButton moveDownButton;

	// Element to update on change.
	Control callback;

	public void Init(ResourcePool _resourcePool){
		resourcePool=_resourcePool;
	}

	public override void _Ready(){
		base._Ready();
	}

	public override void _Draw()
	{	
		if (tradeRoute == null){return;}
		moveUpButton.Disabled = false;
		moveDownButton.Disabled = false;
		// if (tradeRoute.GetIndex() == 0){
		// 	moveUpButton.Disabled = true;
		// }
		// if (tradeRoute.GetIndex() == ((resourcePool.GetTradeRoutes().Count())-1){
		// 	moveDownButton.Disabled = true;
		// }
	}

	public void Init(TradeRoute _tradeRoute, Control _callback){
		tradeRoute = _tradeRoute;
		callback = _callback;

		// Add details panel.
		foreach (Resource r in tradeRoute.poolSource.GetStandard()){
			UIResource ui = resourceIcon.Instance<UIResource>();
			ui.Init(r);
			GetNode("Details").AddChild(ui);
		}
		GetNode("Summary").Connect("toggled", this, "ShowDetails");

		// Set button text
		GetNode<Label>("Summary/AlignLeft/Source").Text = $"→ system - {tradeRoute.poolSource.GetParent<Body>().Name}";
		UIResource freighterIcon = GetNode<UIResource>("Summary/AlignLeft/Freighters");
		freighterIcon.Init(tradeRoute.tradeWeight);

		// Set reorder buttons
		moveUpButton = GetNode<TextureButton>("Summary/AlignRight/Reorder/MoveUp");
		moveDownButton = GetNode<TextureButton>("Summary/AlignRight/Reorder/MoveDown");
		moveUpButton.Connect("pressed", this, "ReorderUp");
		moveDownButton.Connect("pressed", this, "ReorderDown");

		// Set reorder buttons
		GetNode<TextureButton>("Summary/AlignRight/Cancel/").Connect("pressed", this, "Remove");
	}

	public void ShowDetails(bool toggled){
		GetNode<Control>("Details").Visible = toggled;
	}
	public void Remove(){
		GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(tradeRoute);
		GetParent().RemoveChild(this);
		callback.Update();
		this.QueueFree();
	}

	public void ReorderUp(){
		resourcePool.MoveChild(tradeRoute, tradeRoute.GetIndex()-1);
		callback.Update();
	}
	public void ReorderDown(){
		resourcePool.MoveChild(tradeRoute, tradeRoute.GetIndex()+1);
		callback.Update();
	}
}
