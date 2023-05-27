using Godot;
using System;

public partial class PlayerTradeRoutes : Node
{
    static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");
    public void RegisterTradeRoute(Installation destination, Installation source)
    {
        TradeRoute newTradeRoute = ps_TradeRoute.Instantiate<TradeRoute>();
        newTradeRoute.Init(destination, source);
        AddChild(newTradeRoute);
    }
    public void DeregisterTradeRoute(TradeRoute tr)
    {
        tr.source.DeregisterConsumer(tr.IndustrySource);
        tr.destination.DeregisterConsumer(tr.IndustryDestintation);

        tr.destination.uplineTraderoute = null;

        RemoveChild(tr);
        tr.QueueFree();
        GD.Print("Removed trade route.");
    }
}
