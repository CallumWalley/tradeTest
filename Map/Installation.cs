using Godot;
using System;
using System.Collections.Generic;

public partial class Installation : EcoNode
{

    [Export]
    public bool isValidTradeReceiver = false;

    [Export]
    public bool storageFull = false;
    List<Industry> industries = new();
    List<Resource.IResourceConsumer> consumers = new();

    public List<Resource.IResourceConsumer> Consumers { get { return consumers; } }

    public List<Industry> Industries { get { return industries; } }

    //for debuggin
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    public List<string> tags;

    // contains only POSITIVE deltas. evaluated during EFrameLate, and cleared in next EFrameEarly.
    public Resource.RGroupList<Resource.RGroup> resourceDeltaProduced = new();
    // contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
    public Resource.RGroupList<Resource.RGroup> resourceDeltaConsumed = new();
    // Contains sum of deltas, used by UI elements.
    public Resource.RGroupList<Resource.RGroup> resourceDelta = new();
    // NET amount of resource in storage.
    public Resource.RStorageList<Resource.RStorage> resourceStorage = new();
    // TOTAL storage available.

    // Downline traderoutes

    // Upline traderoute
    public TradeRoute uplineTraderoute;
    public List<TradeRoute> downlineTraderoutes;


    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();
        resourceDeltaProduced.CreateMissing = true;
        resourceDeltaConsumed.CreateMissing = true;
        resourceDelta.CreateMissing = true;
        resourceStorage.CreateMissing = true;

        if (isValidTradeReceiver)
        {
            GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").RegisterInstallation(this);
        }
        // If added in inspector. Regester Industrys.
        foreach (Industry t in Children)
        {
            // Remove self so not trigger warning when added.
            RemoveChild(t);
            RegisterIndustry(t);
        }
    }
    public void RegisterConsumer(Resource.IResourceConsumer c)
    {
        Consumers.Add(c);
        //AddChild(tr);

        // foreach (Resource.RGroup p in tr.Production)
        // {
        //     resourceDeltaProduced.Add((Resource.RGroup)p);
        //     resourceDelta.Add((Resource.RGroup)p);
        // }
        // foreach (Resource.BaseRequest p in tr.Consumption)
        // {
        //     resourceDeltaConsumed.Add(p.Response);
        //     resourceDelta.Add(p.Response);
        // }
        // foreach (Resource.RStatic p in tr.Storage)
        // {
        //     resourceStorage[p.Type()].Add(p);
        //     resourceStorage[p.Type()].Fill();
        // }
    }
    public void DeregisterConsumer(Resource.IResourceConsumer c)
    {
        Consumers.Remove(c);
        // RemoveChild(tr);

        // foreach (Resource.RGroup p in tr.Production)
        // {
        //     resourceDeltaProduced.Remove(p);
        //     resourceDelta.Remove(p);

        // }
        // foreach (Resource.BaseRequest p in tr.Consumption)
        // {
        //     resourceDeltaConsumed.Remove(p.Response);
        //     resourceDelta.Remove(p.Response);

        // }
        // foreach (Resource.RStorage p in tr.Storage)
        // {
        //     resourceStorage.Remove(p);
        // }
    }
    public void RegisterIndustry(Industry i)
    {
        AddChild(i);
        RegisterConsumer(i.Consumer);
    }
    public void DeregisterIndustry(Industry i)
    {
        RemoveChild(i);
        DeregisterConsumer(i.Consumer);
    }
    // public IEnumerable<IndustryTrade> GetTradeRoutes()
    // {
    //     foreach (Industry t in GetChildren())
    //     {
    //         if (t is IndustryTrade)
    //         {
    //             yield return IndustryTrade;
    //         }
    //     }
    // }

    // this is so messy, i hate it. aaaaaaaaaaah
    public override void EFrameLate()
    {

        // // Re count storage
        // foreach (Resource.RGroup r in resourceStorage)
        // {
        // 	r.Clear();
        // }
        // //Get production from each Industry.
        // foreach (Resource r in resourceDeltaProduced)
        // {
        // 	r.Clear();
        // }
        // foreach (Industry Industry in GetChildren())
        // {
        // 	foreach (Resource r in Industry.Production)
        // 	{
        // 		resourceDeltaProduced.Add(r);
        // 		resourceDelta.Add(r);
        // 	}
        // 	foreach (Resource r in Industry.Storage)
        // 	{
        // 		resourceStorage.Add(r);
        // 	}
        // }
    }
    public override void EFrameEarly()
    {
        Dictionary<int, double> lastProduced = new();
        foreach (Resource.RBase r in resourceDeltaProduced)
        {
            lastProduced[r.Type()] = r.Sum();
        }

        foreach (Resource.IResourceConsumer rc in Consumers)
        {
            foreach (Resource.BaseRequest input in rc.Consumed())
            {
                int type = input.Request.Type();
                // How much of this resource was produced last step.
                Resource.RStorage storage = (Resource.RStorage)resourceStorage[type];
                double requested = input.Request.Sum();
                // this should just be somewhere else.
                if (!lastProduced.ContainsKey(type))
                {
                    lastProduced[type] = 0;
                }
                // Amount of extra DELTA required to cover this request.
                double remainderDelta = (lastProduced[type] + requested); //+ it.Request.Sum();

                // GD.Print(String.Format("Produced: {0:N1}\nConsumed: {1:N1}\nRequested: {2:N1}\nDelta: {3:N1}\nStored: {4:N1}\nRemainder: {5:N1}", produced, consumed, requested, remainderDelta, stockpile, remainderNet));
                // Amount of extra NET required to cover this request.	
                if (remainderDelta >= 0)
                {
                    input.Respond();
                    lastProduced[type] += requested;
                }
                else if (storage.Stock() >= -remainderDelta)
                {
                    input.Respond();
                    //storage.Deposit(remainderDelta);
                    lastProduced[type] += requested;
                }
                else
                {
                    // Fullfill partial request.
                    input.Respond(-storage.Stock());
                    //storage.Deposit(-storage.Stock());
                    lastProduced[type] = 0;
                }

                // Add response to lists.
                // resourceDeltaConsumed.Add(it.Response);
                // resourceDelta.Add(it.Response);

                // Deduct remainder from storage.
                // Emit some sort of storage message.
            }
        }
        foreach (KeyValuePair<int, double> kvp in lastProduced)
        {
            if (resourceStorage.ContainsKey(kvp.Key))
            {
                resourceStorage[kvp.Key].Deposit(kvp.Value);
            }
        }
    }
}
