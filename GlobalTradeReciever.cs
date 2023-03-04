using Godot;
using System;

public class GlobalTradeReciever : Node
{
    [Export]
    public Godot.Collections.Array<TradeReceiver> _index;

    public GlobalTradeReciever()
    {
        _index = new Godot.Collections.Array<TradeReceiver>();
    }

    public void Register(TradeReceiver tr){
        _index.Add(tr);
    }

    public Godot.Collections.Array<TradeReceiver> List(){
        return _index;
    }
}
