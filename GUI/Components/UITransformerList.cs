using Godot;
using System;
using System.Collections.Generic;

public class UITransformerList : Control
{

	VBoxContainer list;
	ResourcePool resourcePool;
	static readonly Texture freighterIcon = GD.Load<Texture>("res://assets/icons/freighter.png");
	//static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeRoute.tscn");
	static readonly PackedScene p_uitransformer = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITransformer.tscn");

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
		foreach (Transformer tr in resourcePool.GetChildren()){
			UpdateTradeRoute((Transformer)tr,index);
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

	void UpdateTradeRoute(Transformer tr, int index){
		//Check element with this trade route doesn't already exist.
		foreach (UITransformer uit in list.GetChildren()){
			if (uit.transformer == tr){
				list.MoveChild(uit, index);
				return;
			}
		}
		// If doesn't exist, add it and insert at postition.
		UITransformer ui = (UITransformer)p_uitransformer.Instance();
		ui.Init(tr);
		list.AddChild(ui);
		list.MoveChild(ui, index);
	}
}
