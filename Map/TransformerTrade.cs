using Godot;
using System;
using System.Collections.Generic;

public class TransformerTrade : Transformer
{ 
    public TradeRoute tradeRoute;

    public bool isSource;    

    public override void _Ready()
    {
        base._Ready();  
    }

    public void Init(TradeRoute _tradeRoute, bool _isSource=false){
        tradeRoute=_tradeRoute;
        isSource = _isSource;
        ttype = TransformerRegister.TradeRoute;
        Tags = new string[]{"trade_route", "non_buildable"};
        Description = $"A Trade Route connecting {tradeRoute.destination.Name}, {tradeRoute.destination.Body.Name} to {tradeRoute.source.Name}, {tradeRoute.source.Body.Name}";

        if (isSource){
            Name = $"Trade to {tradeRoute.destination.Body.Name}";
        }else{
            Name = $"Trade from {tradeRoute.source.Body.Name}";
        }
    }

    // BUG: Changing resource on destination does not work as is passed phoney list.
    public override IEnumerable<Resource> Produced(){
        if (isSource){
            return tradeRoute.BalanceSource;
        }else{
            return tradeRoute.BalanceDestination;
        }
    }
}
