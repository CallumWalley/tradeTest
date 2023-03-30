using Godot;
using System;
using System.Collections.Generic;

public class TransformerTrade : Transformer
{ 
    public TradeRoute tradeRoute;

    public bool isSource;

    // [Export]
    Resource upkeep;

    // [Export]
    Resource operationCost;
    
    // [Export]
    Resource production;

    

    public override void _Ready()
    {
        base._Ready();  
    }

    public void Init(TradeRoute _tradeRoute, bool _isSource=false){
        tradeRoute=_tradeRoute;
        isSource = _isSource;
    }

    public new IEnumerable<Resource> Production(){
        if (isSource){
            return tradeRoute.BalanceSource;
        }else{
            GD.Print("inverted");
            return tradeRoute.BalanceDestination;
        }
    }
}
