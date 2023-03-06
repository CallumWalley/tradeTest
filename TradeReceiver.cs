using Godot;
using System;

public class TradeReceiver : EcoNode
{   
	public ResourceAgr freightersRequired;
	public ResourceStatic freightersTotal;

	public ResourcePool resourcePool;

	public float freighterCapacity = 14;
	public string name = "Trade Station";
	static readonly PackedScene tradeReceiverUI = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/UITradeReceiver.tscn");
	static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://templates/TradeRoute.tscn");
	static readonly PackedScene p_resourcePool = (PackedScene)GD.Load<PackedScene>("res://templates/ResourcePool.tscn");

		
	public override void _Ready(){
		base._Ready();
		// If parent doesn't have resource pool. add one.
		resourcePool = GetNodeOrNull<ResourcePool>("../ResourcePool");
		if (resourcePool==null){
			resourcePool = p_resourcePool.Instance<ResourcePool>();
			GetParent().AddChild(resourcePool);
		}

		//Register with global trade ledger.
		GetNode<GlobalTradeReciever>("/root/Global/Trade").Register(this);

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

		// Create UI
		Control uiParent = (Control)GetNode("../InfoCard");
		UITradeReceiver ui = tradeReceiverUI.Instance<UITradeReceiver>();
		ui.tradeReceiver = this;
		uiParent.AddChild(node: ui);
		EconomyFrame();
	}

	public void RegisterTradeRoute(TradeSource ts){
		TradeRoute newTradeRoute = tradeRoute.Instance<TradeRoute>();
		newTradeRoute.Init(ts, this);
		AddChild(newTradeRoute);
		ts.tradeRoute = newTradeRoute;
		freightersRequired._add.Add(newTradeRoute.tradeWeight);
	}
	public void DeregisterTradeRoute(TradeRoute tr){
		try
		{
			tr.tradeSource.tradeRoute = null;
			freightersRequired._add.Remove(tr.tradeWeight);
			RemoveChild(tr);
			tr.QueueFree(); 
		}
		catch (System.Exception)
		{    
			return;
		}

	}

	public Vector2 Position {
		get { return GetParent<Body>().Position; }
	}
}
