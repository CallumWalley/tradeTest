using Godot;
using System;

public class TradeRoute : EcoNode
{
	[Export]
	public ResourcePool resourcePool;
	public TradeReceiver tradeReceiver;

	public Body body;
	// Tradeweight in KTonnes
	//public Resource importTradeWeight;
	//public Resource exportTradeWeight;
	public Resource tradeWeight;
	public float distance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{   
		base._Ready();
		DrawLine();
	}
	// Called from TradeReciver
	public void Init(Body _body, ResourcePool _resourcePool, TradeReceiver _tradeReceiver){
		resourcePool=_resourcePool;
		tradeReceiver=_tradeReceiver;
		body = _body;
		distance = body.Position.DistanceTo(tradeReceiver.Position);
		tradeWeight = new ResourceStatic();
		tradeWeight.Type = 901;
		tradeWeight.Name = $"Trade route from {Name}";
		AddChild(tradeWeight);
		foreach (ResourceAgr item in resourcePool.GetStandard())
		{
			tradeReceiver.resourcePool.Add(item);
		}
	}

	public override void EFrameCollect()
	{   
		tradeWeight.Sum = GetNode<GlobalTech>("/root/Global/Tech").GetFreighterTons(resourcePool.shipWeight, distance);
	}

	public void DrawLine(){
		GetNode<Line2D>("Line2D").Points=new Vector2[]{body.Position, tradeReceiver.Position};
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
