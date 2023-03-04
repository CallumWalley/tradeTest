using Godot;
using System;

public class TradeReceiver : EcoNode
{   
    public ResourceAgr freightersRequired;
    public ResourceStatic freightersTotal;
    public ResourceAgr freighterPool;

    public ResourcePool resourcePool;

    public float freighterCapacity = 14;
    public string name = "Trade Station";
    static readonly PackedScene tradeReceiverUI = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/UITradeReceiver.tscn");
    static readonly PackedScene tradeRoute = (PackedScene)GD.Load<PackedScene>("res://templates/TradeRoute.tscn");
    static readonly PackedScene p_resourcePool = (PackedScene)GD.Load<PackedScene>("res://templates/ResourcePool.tscn");

	public void Ready(){
        resourcePool = GetNodeOrNull<ResourcePool>("ResourcePool");
        if (resourcePool==null){}
        resourcePool = _resourcePool;
        //Register with global trade ledger.
        GetNode<GlobalTradeReciever>("/root/Global/Trade").Register(this);

        // Create freigher resource.
        // Magic freighters
        freighters = new ResourceAgr();
        freightersRequired = new ResourceAgr();
        freightersTotal= new ResourceStatic();

        freighters.Type=901;
        freighters.Name="Net Freighters";
        freightersRequired.Type=901;
        freightersRequired.Name="Required Freighters";
        freightersTotal.Type=901;
        freightersTotal.Sum=freighterCapacity;
        freightersTotal.Name="Magic Freighters";

        resourcePool.AddChild(freighters);
        resourcePool.AddChild(freightersRequired);
        resourcePool.AddChild(freightersTotal);

        freighters._add.Add(freightersTotal);
        freighters._sub.Add(freightersRequired);

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
