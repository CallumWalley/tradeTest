using Godot;
using System;

public class UITradeRoute : Control
{
	public TradeRoute tradeRoute;
 	static readonly PackedScene tradeSource = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/UITradeSource.tscn");
	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/Components/UIResource.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Draw()
	{
		// Create UI
	}

	public void Init(TradeRoute  tradeRoute){
		foreach (Resource r in tradeRoute.tradeSource.resourcePool.GetChildren()){
			UIResource ui = resourceIcon.Instance<UIResource>();
			ui.Init(r);
			GetNode("TradeRoute/Details").AddChild(ui);
		}
		GetNode("TradeRoute/Summary").Connect("toggled", this, "ShowDetails");


		GetNode<Label>("TradeRoute/Summary/Source").Text = $"→ system - {tradeRoute.tradeSource.Name}";
		UIResource freighterIcon = GetNode<UIResource>("TradeRoute/Summary/Freighters");
		freighterIcon.Init(tradeRoute.tradeWeight);
	}

	public void ShowDetails(bool toggled){
		GetNode<Control>("TradeRoute/Details").Visible = toggled;
	}

}
