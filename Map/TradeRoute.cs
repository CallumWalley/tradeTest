using Godot;
using System;
using System.Collections.Generic;

public class TradeRoute : ResourcePool
{
	[Export]
	public ResourcePool poolSource;
	public ResourcePool poolDestination;
	public TransformerTrade transformerSource;
	public TransformerTrade transformerDestintation;

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
	public void Init(ResourcePool _poolSource, ResourcePool _poolDestination){

		poolSource = _poolSource;
		poolDestination=_poolDestination;

		transformerSource = poolSource.GetTransformerTrade();
		transformerDestintation = poolSource.GetTransformerTrade();

		transformerSource.tradeRoutes.Add(this);
		transformerDestintation.tradeRoutes.Add(this);
		poolSource.tradeRoute = this;

		distance = poolSource.GetParent<Body>().Position.DistanceTo(poolDestination.GetParent<Body>().Position);
		tradeWeight = new ResourceStatic(901, 0);
		//tradeWeight.Name = $"Trade route from {Name}";
	}

	public override void EFrameCollect()
	{   
		tradeWeight.Sum = GetNode<PlayerTech>("/root/Global/Player/Tech").GetFreighterTons(poolSource.shipWeight, distance);
	}

	public void DrawLine(){
		GetNode<Line2D>("Line2D").Points=new Vector2[]{poolSource.GetParent<Body>().Position, poolDestination.GetParent<Body>().Position};
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
