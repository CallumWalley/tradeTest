using Godot;
using System;

public class TradeRoute : EcoNode
{
    [Export]
    public TradeSource tradeSource;
    public TradeReceiver tradeReceiver;

    // Tradeweight in KTonnes
    //public Resource importTradeWeight;
    //public Resource exportTradeWeight;
    public Resource tradeWeight;
    public float distance;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {   
        base._Ready();
        DrawLine();
    }
    // Called from TradeReciver
    public void Init(TradeSource _tradeSource, TradeReceiver _tradeReceiver){
        tradeSource=_tradeSource;
        tradeReceiver=_tradeReceiver;
        distance = tradeSource.Position.DistanceTo(tradeReceiver.Position);
        tradeWeight = new ResourceStatic();
        tradeWeight.Type = 901;
        tradeWeight.Name = $"Trade route from {Name}";
        AddChild(tradeWeight);

    }

    public override void EconomyFrame()
    {   
        tradeWeight.Sum = GetNode<GlobalTech>("/root/Global/Tech").GetFreighterTons(tradeSource.shipWeight, distance);
    }

    public void DrawLine(){
        GetNode<Line2D>("Line2D").Points=new Vector2[]{tradeSource.Position, tradeReceiver.Position};
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
