using Godot;
using System;
using System.Collections.Generic;

public class TradeRoute : EcoNode
{
	[Export]
	public ResourcePool poolDestination;
	public ResourcePool poolSource;
	public TransformerTrade transformerSource;
	public TransformerTrade transformerDestintation;
	public List<Resource> BalanceSource{get; private set;} = new List<Resource>();
	public List<Resource> BalanceDestination{get{
		return new List<Resource>( InvertBalance());
	}}

	public Body Body{get{return GetParent<Body>();}}
	public int Index{get{return GetIndex();}}


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
	public void Init(ResourcePool _poolDestination, ResourcePool _poolSource){

		poolDestination = _poolDestination;
		poolSource = _poolSource;

		transformerDestintation = new TransformerTrade();
		transformerSource = new TransformerTrade();

		transformerSource.Init(this, true);
		transformerDestintation.Init(this);

		poolSource.RegisterTransformer(transformerDestintation);
		poolDestination.RegisterTransformer(transformerSource);
		poolDestination.uplineTraderoute = this;

		distance = poolDestination.GetParent<Body>().Position.DistanceTo(poolSource.GetParent<Body>().Position);
		MatchDemand();
		tradeWeight = new ResourceStatic(901, 0);
		//tradeWeight.Name = $"Trade route from {Name}";
	}

	// public override void EFrameCollect()
	// {   
	// 	tradeWeight.Sum = GetNode<PlayerTech>("/root/Global/Player/Tech").GetFreighterTons(poolDestination.shipWeight, distance);
	// }

	public void DrawLine(){
		GetNode<Line2D>("Line2D").Points=new Vector2[]{poolDestination.GetParent<Body>().Position, poolSource.GetParent<Body>().Position};
	}
	public float GetShipWeight(){
		float shipWeightImport = 0;
		float shipWeightExport = 0;
		foreach (Resource child in BalanceSource){
			if (child.Sum > 0){
				shipWeightExport += child.Sum * Resources.ShipWeight(child.Type);
			} else {
				shipWeightImport += child.Sum * Resources.ShipWeight(child.Type);
			}
		}
		return Math.Max(shipWeightExport, shipWeightImport);
	}

	IEnumerable<Resource> InvertBalance(){

		foreach (Resource r in BalanceSource){
			yield return new ResourceStatic(r.Type, -r.Sum);
		}
	}

	public void MatchDemand(){
		BalanceSource.Clear();
		foreach (Resource r in poolDestination.GetStandard()){
			BalanceSource.Add(new ResourceStatic(r.Type, r.Sum));
		} 
	}
}
