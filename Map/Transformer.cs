using Godot;
using System;

public class Transformer : EcoNode
{ 
    public Resource output;

    // [Export]
    Resource upkeep;

    // [Export]
    Resource operationCost;
    
    // [Export]
    Resource production;

    public bool isTradeReceiver;

    public override void _Ready()
    {
        base._Ready();
        upkeep = new ResourceStatic(2, -1);
        operationCost = new ResourceStatic(1, -2);
        production = new ResourceStatic(4, 2);
    }

    public Resource[] Upkeep(){
        return new Resource[]{ upkeep };
    }

    public Resource[] OperationCost(){
        return new Resource[]{ operationCost };
    }

    public virtual Resource[] Production(){
        return new Resource[]{ production };
    }
}
