using Godot;
using System;

public class UITradeReceiver : Control
{

	VBoxContainer list;
	UIResource freightersTotal;
	UIResource freightersRequired;

	ResourcePool resourcePool;
	static readonly Texture freighterIcon = GD.Load<Texture>("res://assets/icons/freighter.png");
	static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeRoute.tscn");

	public void Init(ResourcePool _resourcePool){
		resourcePool = _resourcePool;
	}

	public override void _Ready()
	{
		// Assign freighter counting icons.
		// freightersTotal = GetNode<UIResource>("FreighterPool/Pool");
		// freightersRequired = GetNode<UIResource>("FreighterPool/Used");
		// freightersTotal.Init(tradeReceiver.freightersTotal);
		// freightersRequired.Init(tradeReceiver.freightersRequired);
		//list = GetNode<VBoxContainer>("Scroll/VBox");     
		list = GetNode<VBoxContainer>("NoScroll/VBox");        
	}
	
	public override void _Draw(){
		Clear();
		//TODO proper string formatting.
		//freighterPoolLabel.Text = $"[ {tradeReceiver.freightersUsed.Sum.ToString()} / {tradeReceiver.freighterCapacity.ToString()}]";
		if (resourcePool.GetTransformerTrade().tradeRoutes.Count < 1){
			ResourcePool rp = resourcePool;
			Label noNodeLabel = new Label();
			noNodeLabel.Text = "No Trade Routes";
			noNodeLabel.Valign = Label.VAlign.Center;
			noNodeLabel.Align = Label.AlignEnum.Center;
			list.AddChild(noNodeLabel);
		}
		else
		{
			foreach (TradeRoute tr in resourcePool.GetTransformerTrade().tradeRoutes){
				UITradeRoute ui = (UITradeRoute)tradeRoute.Instance();
				list.AddChild(ui);
				ui.Init(tr, this);
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
