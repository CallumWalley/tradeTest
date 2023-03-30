using Godot;
using System;
using System.Collections.Generic;

public class ResourcePool : EcoNode
{  

	// public ResourceAgr freightersRequired;
	// public ResourceStatic freightersTotal;
	// public ResourcePool resourcePool;

	// public float freighterCapacity = 14;
	public string name = "Trade Station";
	// static readonly PackedScene ps_tradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

	[Export]
	public bool isValidTradeReceiver = false;

	public List<string> tags;

	public List<Resource> members = new List<Resource>();

	// Downline traderoutes
	public TransformerTrade transformerTrade;

	// Upline traderoute
	public TradeRoute uplineTraderoute;

	public Vector2 Position{get{return GetParent<Body>().Position;}}

	// BODY IS PARENT
	//public Body body;

	public float shipWeight;

	// Convenience function. Makes children at start of scene into members.
	public override void _Ready(){
		base._Ready();
		if (isValidTradeReceiver){
			GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").RegisterResourcePool(this);
		}
	}
	public void RegisterTransformer(Transformer tr){
		AddChild(tr);
	}
	public void DeregisterTransformer(Transformer tr){
		RemoveChild(tr);
	}
	public IEnumerable<TransformerTrade> GetTradeRoutes(){
		foreach (Transformer t in GetChildren())
		if (t is TransformerTrade){
			yield return  transformerTrade;
		}
	}

	public ResourceAgr GetType(int code){
		foreach (ResourceAgr r in members){
			if (r.Type == code){
				return r;
			}
		} 
		ResourceAgr newResource = new ResourceAgr(code, new List<Resource>{} );
		members.Add(newResource);
		return newResource;
	}
	public IEnumerable<ResourceAgr> GetIncome(){
		foreach (ResourceAgr r in members){
			if (r.Sum > 0){
				yield return r;
			}
		} 
	}
	public IEnumerable<ResourceAgr> GetExpenses(){
		foreach (ResourceAgr r in members){
			if (r.Sum < 0){
				yield return r;
			}
		} 
	}
	// public IEnumerable<Transformer> GetTradeRoutes(){
	// 	foreach (Transformer t in GetChildren()){
	// 		if (t is TransformerTrade){
	// 			yield return t;
	// 		} 
	// 	}
	// }
	// Get resources with code between range.
	public IEnumerable<ResourceAgr> GetRange(int min, int max){
		foreach (ResourceAgr r in members){
			if (min <= r.Type &&  r.Type <= max){
					yield return r;
			} 
		}
	}
	
	public IEnumerable<ResourceAgr> GetStandard(){
		return GetRange(1, 100);
	}
	public override void EFrameCollect(){
		members.Clear();
		foreach (Transformer transformer in GetChildren()){
			foreach (Resource r in transformer.Upkeep()){
				AddResource(r);
			}
			foreach (Resource r in transformer.OperationCost()){
				AddResource(r);
			}
			foreach (Resource r in transformer.Production()){
				AddResource(r);
			}		
		}
		// GD.Print($"{Name} has {members.Count} members");
	}

	private void AddResource(Resource _resource){
		//Check if aggrigator already exists.
		GetType(_resource.Type).add.Add(_resource);
	}
}
