using Godot;
using System;
using System.Collections.Generic;

public class TransformerTrade : Transformer
{ 
    public TradeRoute tradeRoute;

    public bool isSource;    

    public override void _Ready()
    {
        ttype = GetNode<TransformerRegister>("/root/Global/TransformerRegister").tradeRoute;

        base._Ready();  

        //public int Prioroty {get;set;}
    }

    public void Init(TradeRoute _tradeRoute, bool _isSource=false){
        tradeRoute=_tradeRoute;
        isSource = _isSource;
        Tags = new string[]{"trade_route", "non_buildable"};
        Description = "A trade route";
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
