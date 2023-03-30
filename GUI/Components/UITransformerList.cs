using Godot;
using System;
using System.Collections.Generic;

public class UITransformerList : Control
{

	VBoxContainer list;
	UIResource freightersTotal;
	UIResource freightersRequired;

	List<TradeRoute> lastDraw;

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
		//Clear();


		// //TODO proper string formatting.
		// //freighterPoolLabel.Text = $"[ {tradeReceiver.freightersUsed.Sum.ToString()} / {tradeReceiver.freighterCapacity.ToString()}]";
		// if (resourcePool.GetTransformerTrade().tradeRoutes.Count < 1){
		// 	Label noNodeLabel = new Label();
		// 	noNodeLabel.Text = "No Trade Routes";
		// 	noNodeLabel.Valign = Label.VAlign.Center;
		// 	noNodeLabel.Align = Label.AlignEnum.Center;
		// 	list.AddChild(noNodeLabel);
		// }
		// else
		// {s

		// Go over all trade routes in pool, and either update or create. 
		int index = 0;
		foreach (TransformerTrade tr in resourcePool.GetTradeRoutes()){
			UpdateTradeRoute((TransformerTrade)tr,index);
			index++;
		}
		// Any remaining elements greater than index must no longer exist.
		while (list.GetChildCount() > index){
			Transformer tr = list.GetChildOrNull<Transformer>(index+1);
			list.RemoveChild(tr);
			tr.QueueFree();
		}
		//}
	}

	public void Clear(){
		foreach (Control tr in list.GetChildren()){
			list.RemoveChild(tr);
			tr.QueueFree();
		}
	} 

	void UpdateTradeRoute(TransformerTrade tr, int index){
		//Check element with this trade route doesn't already exist.
		foreach (UITradeRoute uit in list.GetChildren()){
			if (uit.tradeRoute == tr.tradeRoute){
				list.MoveChild(uit, index);
				return;
			}
		}
		// If doesn't exist, add it and insert at postition.
		UITradeRoute ui = (UITradeRoute)tradeRoute.Instance();
		ui.Init(tr.tradeRoute);
		list.AddChild(ui);
		list.MoveChild(ui, index);
	}
}
