using Godot;
using System;

public class TradeReceiver : EcoNode
{   
	public ResourceAgr freightersRequired;
	public ResourceStatic freightersTotal;
	public ResourcePool resourcePool;

	public float freighterCapacity = 14;
	public string name = "Trade Station";
	static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

	public void Init(ResourcePool _resourcePool){
		resourcePool = _resourcePool;
	}

	public override void _Ready(){
		base._Ready();
		//Register with global trade ledger.
		GetNode<GlobalTradeReciever>("/root/Global/Trade").Register(this);

		Name = $"{GetParent().Name} Trade Station";
		// Create freigher resource.
		// Magic freighters
		freightersRequired = new ResourceAgr();
		freightersTotal= new ResourceStatic();
		freightersRequired.Type=901;
		freightersTotal.Type=901;
		freightersRequired.Name="Required Freighters";
		freightersTotal.Name="Magic Freighters";
		freightersTotal.Sum=freighterCapacity;

		resourcePool.AddChild(freightersRequired);
		resourcePool.AddChild(freightersTotal);
	}

	public void RegisterTradeRoute(Body body, ResourcePool rp){
		TradeRoute newTradeRoute = tradeRoute.Instance<TradeRoute>();
		newTradeRoute.Init(body, rp, this);
		AddChild(newTradeRoute);
		rp.tradeRoute = newTradeRoute;
		freightersRequired._add.Add(newTradeRoute.tradeWeight);
	}
	public void DeregisterTradeRoute(TradeRoute tr){
//		try
//		{
			freightersRequired._add.Remove(tr.tradeWeight);

			tr.resourcePool.tradeRoute = null;
			GD.Print(tr.resourcePool.members.ToString());
			foreach (ResourceAgr item in tr.resourcePool.GetStandard())
			{	
				//int indx = resourcePool.members.IndexOf(item);
				GD.Print(resourcePool.members.Remove(item));
			}
			RemoveChild(tr);
			tr.QueueFree();
			GD.Print("Removed trade route.");
//		}
//		catch (System.Exception)
//		{    
//			GD.Print("Failed to remove trade route.");
//		}
	}

	public Vector2 Position {
		get { return GetParent<Body>().Position; }
	}
}
