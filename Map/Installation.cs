using Godot;
using System;
using System.Collections.Generic;

public class Installation : EcoNode
{

    // public ResourceAgr freightersRequired;
    // public ResourceStatic freightersTotal;
    // public Installation installation;

    // public float freighterCapacity = 14;
    // static readonly PackedScene ps_tradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

    // public Resource GetTypeDelta(int type){
    //     return resourceDelta.GetType(type);
    // }

    [Export]
    public bool isValidTradeReceiver = false;

    //for debuggin
    public Godot.Collections.Array Children { get { return GetChildren(); } }

    public List<string> tags;

    // contains only POSITIVE deltas. evaluated during EFrameLate, and cleared in next EFrameEarly.
    public ResourceList resourceDeltaProduced = new ResourceList();
    // contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
    public ResourceList resourceDeltaConsumed = new ResourceList();
    // Contains sum of deltas, used by UI elements.
    public ResourceList resourceDelta = new ResourceList();

    // NET amount of resource in storage.
    public ResourceList resourceStockpile = new ResourceList(true);
    // TOTAL storage available.
    public ResourceList resourceStorage = new ResourceList();


    // Downline traderoutes
    public TransformerTrade transformerTrade;

    // Upline traderoute
    public TradeRoute uplineTraderoute;

    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public float shipWeight;

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();
        if (isValidTradeReceiver)
        {
            GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").RegisterInstallation(this);
        }
    }
    public void RegisterTransformer(Transformer tr)
    {
        AddChild(tr);
    }
    public void DeregisterTransformer(Transformer tr)
    {
        RemoveChild(tr);
    }
    public IEnumerable<TransformerTrade> GetTradeRoutes()
    {
        foreach (Transformer t in GetChildren())
            if (t is TransformerTrade)
            {
                yield return transformerTrade;
            }
    }


    // this is so messy, i hate it. aaaaaaaaaaah
    public override void EFrameLate()
    {
        foreach (Resource r in resourceDelta)
        {
            Resource typeStockpile = resourceStockpile.GetType(r.Type, true);
            Resource typeStorage = resourceStorage.GetType(r.Type, true);

            typeStockpile.Sum = Math.Min(Math.Max(typeStockpile.Sum + r.Sum, 0), typeStorage.Sum);
            r.Clear();
        }
        // Re count storage
        foreach (ResourceAgr r in resourceStorage)
        {
            r.Clear();
        }
        //Get production from each transformer.
        foreach (Resource r in resourceDeltaProduced)
        {
            r.Clear();
        }
        foreach (Transformer transformer in GetChildren())
        {
            foreach (Resource r in transformer.Production)
            {
                resourceDeltaProduced.Add(r);
                resourceDelta.Add(r);
            }
            foreach (Resource r in transformer.Storage)
            {
                resourceStorage.Add(r);
            }
        }
    }
    public override void EFrameEarly()
    {
        foreach (Resource r in resourceDeltaConsumed)
        {
            r.Clear();
        }
        foreach (Transformer transformer in GetChildren())
        {
            foreach (TransformerInputType.Base it in transformer.Consumption)
            {
                int type = it.Type;
                // How much of this resource was produced last step.
                float produced = resourceDeltaProduced.GetType(type, true).Sum;
                float consumed = resourceDeltaConsumed.GetType(type, true).Sum;
                float stockpile = resourceStockpile.GetType(type, true).Sum;
                float storage = resourceStorage.GetType(type, true).Sum;

                // // Amount of extra DELTA required to cover this request.
                // float remainderDelta = (produced + consumed) + r.Sum;
                // // Amount of extra NET required to cover this request.	
                // float remainderNet = stockpile + remainderDelta;
                // ResourceStatic rr;
                // int status;
                // if (remainderDelta >= 0 || remainderNet >= 0)
                // {
                //     // Fullfill full request.
                //     rr = new ResourceStatic(r.Type, r.Sum);
                //     status = 0;
                // }
                // else
                // {
                //     // Fullfill partial request.
                //     rr = new ResourceStatic(r.Type, r.Sum + remainderNet);
                //     status = 1;
                // }
                // r.Respond(rr, status);
                // resourceDeltaConsumed.Add(rr);
                // resourceDelta.Add(rr);

                // Deduct remainder from storage.
                // Emit some sort of storage message.
            }
        }
    }
}
