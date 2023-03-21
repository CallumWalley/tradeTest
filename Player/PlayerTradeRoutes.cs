using Godot;
using System;

public class PlayerTradeRoutes : Node
{
    static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");
	public void RegisterTradeRoute(ResourcePool _poolSource, ResourcePool _poolDestination){
		TradeRoute newTradeRoute = ps_TradeRoute.Instance<TradeRoute>();
		newTradeRoute.Init(_poolSource, _poolDestination);
		AddChild(newTradeRoute);
	}
	public void DeregisterTradeRoute(TradeRoute tr){
		tr.poolDestination.RemoveChild(tr.transformerDestintation);
		tr.poolSource.RemoveChild(tr.transformerSource);

		RemoveChild(tr);
		tr.QueueFree();
		GD.Print("Removed trade route.");
	}
}
