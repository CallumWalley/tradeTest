using Godot;
using System;
using System.Collections.Generic;

public class UITradePanel : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Body body;
	UITransformerList UITransformerList;
	UIResourceList UIResourceList;
	UITradeDestination uiTradeDestination;

	static readonly PackedScene p_UITransformerList = GD.Load<PackedScene>("res://GUI/Components/UITransformerList.tscn");
	static readonly PackedScene p_UIResourceList = GD.Load<PackedScene>("res://GUI/Components/UIResourceList.tscn");
	static readonly PackedScene p_uiTradeDestination = GD.Load<PackedScene>("res://GUI/Components/UITradeDestination.tscn");

	public void Init(Body _body){
			// Create trade receiver component.
			body = _body;

			UIResourceList = p_UIResourceList.Instance<UIResourceList>();
			UIResourceList.Init(body.resourcePool.members);
			AddChild(UIResourceList);
			
			uiTradeDestination = p_uiTradeDestination.Instance<UITradeDestination>();
			uiTradeDestination.Init(body, body.resourcePool);
			AddChild(uiTradeDestination);

			UITransformerList = p_UITransformerList.Instance<UITransformerList>();
			UITransformerList.Init(body.resourcePool);

			AddChild(UITransformerList);
			if (body.resourcePool.isValidTradeReceiver){
				UITransformerList.Visible=true;
			}
	}


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
