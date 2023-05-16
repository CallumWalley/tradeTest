using Godot;
using System;
using System.Collections.Generic;

public partial class IndustryTrade : Industry
{
    public TradeRoute tradeRoute;

    //List<IndustryInputType.Base>

    public IndustryTrade twin;

    public bool isSource;

    public override void _Ready()
    {
        base._Ready();
    }

    public void Init(TradeRoute _tradeRoute, bool _isSource = false)
    {
        tradeRoute = _tradeRoute;
        isSource = _isSource;
        if (isSource)
        {
            twin = tradeRoute.IndustryDestintation;
        }
        else
        {
            twin = tradeRoute.IndustrySource;
        }
        ttype = IndustryRegister.TradeRoute;
        Tags = new string[] { "trade_route", "non_buildable" };
        Description = $"A Trade Route connecting {tradeRoute.destination.Name}, {tradeRoute.destination.Body.Name} to {tradeRoute.source.Name}, {tradeRoute.source.Body.Name}";

        if (isSource)
        {
            Name = $"Trade to {tradeRoute.destination.Body.Name}";
        }
        else
        {
            Name = $"Trade from {tradeRoute.source.Body.Name}";
        }
    }
}
