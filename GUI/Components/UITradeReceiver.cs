using Godot;
using System;

public class UITradeReceiver : Control
{

	Tree tree;
	VBoxContainer list;
	UIResource freightersTotal;
	UIResource freightersRequired;
	public TradeReceiver tradeReceiver;
	static readonly Texture freighterIcon = GD.Load<Texture>("res://assets/icons/freighter.png");
	static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeRoute.tscn");

	public void Init(TradeReceiver _tradeReceiver){
		tradeReceiver = _tradeReceiver;
	}

	public override void _Ready()
	{
		// Assign freighter counting icons.
		freightersTotal = GetNode<UIResource>("FreighterPool/Pool");
		freightersRequired = GetNode<UIResource>("FreighterPool/Used");
		freightersTotal.Init(tradeReceiver.freightersTotal);
		freightersRequired.Init(tradeReceiver.freightersRequired);

		list = GetNode<VBoxContainer>("Scroll/VBox");        
	}
	
	public override void _Draw(){
		Clear();
		//TODO proper string formatting.
		//freighterPoolLabel.Text = $"[ {tradeReceiver.freightersUsed.Sum.ToString()} / {tradeReceiver.freighterCapacity.ToString()}]";
		if (tradeReceiver.GetChildCount() < 1){
			Label noNodeLabel = new Label();
			noNodeLabel.Text = "No Trade Routes";
			noNodeLabel.Valign = Label.VAlign.Center;
			noNodeLabel.Align = Label.AlignEnum.Center;
			list.AddChild(noNodeLabel);
		}
		else
		{
			foreach (TradeRoute tr in tradeReceiver.GetChildren()){
				UITradeRoute ui = (UITradeRoute)tradeRoute.Instance();
				ui.Init(tr, this);
				list.AddChild(ui);
			}
		}
	}

	public void Clear(){
		foreach (Control tr in list.GetChildren()){
			list.RemoveChild(tr);
			tr.QueueFree();
		}
	} 
}
