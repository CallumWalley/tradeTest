using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerTradeRoutes : Node
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
    public IEnumerable<TradeRoute> GetChildren()
    {
        foreach (TradeRoute c in base.GetChildren())
        {
            yield return c;
        }
    }
}
