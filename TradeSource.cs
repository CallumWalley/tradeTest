using Godot;
using System;

public class TradeSource : Node
{	
	static PackedScene tradeLedger = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/TradeLedger.tscn");
	public override void _Ready(){
		Control uiParent = (Control)GetNode("../InfoCard");
		UITradeLedger tl = (UITradeLedger)tradeLedger.Instance();
		tl.tradeSource = this;
		uiParent.AddChild(tl);
	}
}
