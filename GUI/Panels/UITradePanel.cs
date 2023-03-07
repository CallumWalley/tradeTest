using Godot;
using System;

public class UITradePanel : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Body body;
	UITradeReceiver uiTradeReceiver;
	UIResourcePool uiResourcePool;
	UITradeDestination uiTradeDestination;

	static readonly PackedScene p_uiTradeReceiver = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeReceiver.tscn");
	static readonly PackedScene p_uiResourcePool = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIResourcePool.tscn");
	static readonly PackedScene p_uiTradeDestination = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeDestination.tscn");

	public void Init(Body _body){
			// Create trade receiver component.
			body = _body;

			uiResourcePool = p_uiResourcePool.Instance<UIResourcePool>();
			uiResourcePool.Init(body.resourcePool);
			AddChild(uiResourcePool);
			
			uiTradeDestination = p_uiTradeDestination.Instance<UITradeDestination>();
			uiTradeDestination.Init(body, body.resourcePool);
			AddChild(uiTradeDestination);

			if (body.tradeReceiver != null){
				uiTradeReceiver = p_uiTradeReceiver.Instance<UITradeReceiver>();
				uiTradeReceiver.Init(body.tradeReceiver);
				AddChild(uiTradeReceiver);
			}
	}


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
