using Godot;
using System;
using System.Collections.Generic;

public class UITransformerList : Control
{

	VBoxContainer list;
	Installation installation;
	//static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeRoute.tscn");
	static readonly PackedScene p_uitransformer = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITransformer.tscn");

	public void Init(Installation _installation){
		installation = _installation;
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
        // if (installation.GetTransformerTrade().tradeRoutes.Count < 1){
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
		foreach (Transformer tr in installation.GetChildren()){
			UpdateTransformer((Transformer)tr,index);
			index++;
		}
		// Any remaining elements greater than index must no longer exist.
		while (list.GetChildCount() > index){
			UITransformer tr = list.GetChildOrNull<UITransformer>(list.GetChildCount()-1);
			list.RemoveChild(tr);
			tr.QueueFree();
		}
		//}
	}

	void UpdateTransformer(Transformer tr, int index){
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
