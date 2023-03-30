using Godot;
using System;
using System.Collections.Generic;

public class Transformer : EcoNode
{ 
    public Resource output;

    // [Export]
    List<Resource> upkeep;

    // [Export]
    List<Resource> operationCost;
    
    // [Export]
    List<Resource> production;

    public bool isTradeReceiver;

    public override void _Ready()
    {
        base._Ready();
        upkeep = new List<Resource>{new ResourceStatic(2, -1)};
        operationCost = new List<Resource>{new ResourceStatic(1, -2)};
        production = new List<Resource>{new ResourceStatic(4, 2)};
    }

    public IEnumerable<Resource> Upkeep(){
        return upkeep;
    }

    public IEnumerable<Resource> OperationCost(){
        return operationCost;
    }

    public virtual IEnumerable<Resource> Production(){
        return  production;
    }

    

    // scaling formula 
    // z = bonus
    // x = number
    // 2z - (2z/x)
}
