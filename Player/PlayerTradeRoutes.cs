using Godot;
using System;

public partial class PlayerTradeRoutes : Node
{
    static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");
    public void RegisterTradeRoute(Installation destination, Installation source)
    {
        TradeRoute newTradeRoute = ps_TradeRoute.Instance<TradeRoute>();
        newTradeRoute.Init(destination, source);
        AddChild(newTradeRoute);
    }
    public void DeregisterTradeRoute(TradeRoute tr)
    {
        tr.source.DeregisterTransformer(tr.transformerSource);
        tr.destination.DeregisterTransformer(tr.transformerDestintation);

        tr.destination.uplineTraderoute = null;

        RemoveChild(tr);
        tr.transformerSource.QueueFree();
        tr.transformerDestintation.QueueFree();
        tr.QueueFree();
        GD.Print("Removed trade route.");
    }
}
