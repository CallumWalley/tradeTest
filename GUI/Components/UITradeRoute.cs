using Godot;
using System;

public class UITradeRoute : Control
{
	public TradeRoute tradeRoute;
	public TradeReceiver tradeReceiver;

	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIResource.tscn");

	TextureButton moveUpButton;
	TextureButton moveDownButton;

	// Element to update on change.
	Control callback;



	// Called when the node enters the scene tree for the first time.
	public override void _Draw()
	{	
		if (tradeRoute == null){return;}
		moveUpButton.Disabled = false;
		moveDownButton.Disabled = false;
		if (tradeRoute.GetIndex() == 0){
			moveUpButton.Disabled = true;
		}
		if (tradeRoute.GetIndex() == tradeReceiver.GetChildCount()-1){
			moveDownButton.Disabled = true;
		}
		// Create UI
	}

	public void Init(TradeRoute _tradeRoute, Control _callback){
		tradeRoute = _tradeRoute;
		tradeReceiver = tradeRoute.GetParent<TradeReceiver>();
		callback = _callback;

		// Add details panel.
		foreach (Resource r in tradeRoute.resourcePool.GetStandard()){
			UIResource ui = resourceIcon.Instance<UIResource>();
			ui.Init(r);
			GetNode("Details").AddChild(ui);
		}
		GetNode("Summary").Connect("toggled", this, "ShowDetails");

		// Set button text
		GetNode<Label>("Summary/AlignLeft/Source").Text = $"→ system - {tradeRoute.body.Name}";
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
		tradeReceiver.DeregisterTradeRoute(tradeRoute);
		GetParent().RemoveChild(this);
		callback.Update();
		this.QueueFree();
	}

	public void ReorderUp(){
		tradeReceiver.MoveChild(tradeRoute, tradeRoute.GetIndex()-1);
		callback.Update();
	}
	public void ReorderDown(){
		tradeReceiver.MoveChild(tradeRoute, tradeRoute.GetIndex()+1);
		callback.Update();
	}
}
