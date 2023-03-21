using Godot;
using System;
using System.Collections.Generic;

public class TransformerTrade : Transformer
{ 
    public List<TradeRoute> tradeRoutes = new List<TradeRoute>();

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

    public void Init(){

    }

    public new Resource[] OperationCost(){
        List<Resource> oc = new List<Resource>();
        foreach (TradeRoute tr in tradeRoutes){
            foreach (Resource r in tr.GetExpenses()){
                oc.Add(r);
            }
        }
        return oc.ToArray();
    }

    public new Resource[] Production(){
        List<Resource> oc = new List<Resource>();
        foreach (TradeRoute tr in tradeRoutes){
            foreach (Resource r in tr.GetIncome()){
                oc.Add(r);
            }
        }
        return oc.ToArray();
    }
}
