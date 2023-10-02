using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerTradeRoutes : Node, IEnumerable<TradeRoute>
{
    static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

    // Initialise trade routes added in editor.
    public override void _Ready()
    {
        foreach (TradeRoute t in GetChildren())
        {
            t.Init();
        }
    }
    public void RegisterTradeRoute(Installation destination, Installation source)
    {
        TradeRoute newTradeRoute = ps_TradeRoute.Instantiate<TradeRoute>();
        newTradeRoute.Init(destination, source);
        AddChild(newTradeRoute);
    }
    public void DeregisterTradeRoute(TradeRoute tr)
    {
        tr.Head.DeregisterDownline(tr);
        tr.Tail.DeregisterUpline(tr);

        RemoveChild(tr);
        tr.QueueFree();
        GD.Print("Removed trade route.");
    }
    public IEnumerator<TradeRoute> GetEnumerator()
    {
        foreach (TradeRoute tradeRoute in GetChildren())
        {
            yield return tradeRoute;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
