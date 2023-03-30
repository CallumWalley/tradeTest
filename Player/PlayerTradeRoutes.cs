using Godot;
using System;

public class PlayerTradeRoutes : Node
{
    static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");
	public void RegisterTradeRoute(ResourcePool poolDestination, ResourcePool poolSource){
		TradeRoute newTradeRoute = ps_TradeRoute.Instance<TradeRoute>();
		newTradeRoute.Init(poolDestination, poolSource);
		AddChild(newTradeRoute);
	}
	public void DeregisterTradeRoute(TradeRoute tr){
		tr.poolSource.RemoveChild(tr.transformerSource);
		tr.poolDestination.RemoveChild(tr.transformerDestintation);
		
		tr.poolDestination.uplineTraderoute = null;


		RemoveChild(tr);
		tr.QueueFree();
		GD.Print("Removed trade route.");
	}
}
