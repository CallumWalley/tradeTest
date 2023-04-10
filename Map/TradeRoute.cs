using Godot;
using System;
using System.Collections.Generic;

public class TradeRoute : EcoNode
{
	[Export]
	public Installation destination;
	public Installation source;
	public TransformerTrade transformerSource;
	public TransformerTrade transformerDestintation;
	public List<Resource> BalanceSource{get; set;} = new List<Resource>();
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
	public void Init(Installation _destination, Installation _source){

		destination = _destination;
		source = _source;

		transformerDestintation = new TransformerTrade();
		transformerSource = new TransformerTrade();

		transformerSource.Init(this, true);
		transformerDestintation.Init(this);

		source.RegisterTransformer(transformerSource);
		destination.RegisterTransformer(transformerDestintation);
		destination.uplineTraderoute = this;

		distance = destination.GetParent<Body>().Position.DistanceTo(source.GetParent<Body>().Position);
		Name = $"Trade route from {transformerSource.Name} to {transformerDestintation.Name}";

		MatchDemand();
		UpdateFreighterWeight();
	}

	public void DrawLine(){
		GetNode<Line2D>("Line2D").Points=new Vector2[]{destination.Position, source.Position};
	}
	public float GetFrieghterWeight(){
		float shipWeightImport = 0;
		float shipWeightExport = 0;
		foreach (Resource child in BalanceSource){
			if (child.Sum > 0){
				shipWeightExport += child.Sum * Resources.ShipWeight(child.Type);
			} else {
				shipWeightImport += child.Sum * Resources.ShipWeight(child.Type);
			}
		}
		return - Math.Max(shipWeightExport, shipWeightImport);
	}

	IEnumerable<Resource> InvertBalance(){
		foreach (Resource r in BalanceSource){
			if (r.Type == 901){continue;}//Ignore trade ship cost.
			yield return new ResourceStatic(r.Type, -r.Sum);
		}
	}

	public void MatchDemand(){
		BalanceSource.Clear();
		foreach (Resource r in destination.GetStandard()){
			BalanceSource.Add(new ResourceStatic(r.Type, r.Sum));
		} 
	}

	void UpdateFreighterWeight(){
		tradeWeight = new ResourceStatic(901, GetFrieghterWeight());
		BalanceSource.Add(tradeWeight);
	}
}
