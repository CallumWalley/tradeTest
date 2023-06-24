using Godot;
using System;
using System.Collections.Generic;

public partial class Installation : EcoNode
{

    [Export]
    public bool ValidTradeReceiver { get; protected set; } = false;

    [Export]
    public Godot.Collections.Dictionary<int, double> StartingResources { get; set; } = new();

    //Transformers contains list trade + industry

    // These are PRIMARY characteristics. To add or remove requires call of function.
    public List<Resource.IResourceTransformers> Transformers { get; } = new();
    public List<Industry> Industries { get; } = new();
    public TradeRoute UplineTraderoute { get; set; }
    public List<TradeRoute> DownlineTraderoutes { get; set; } = new();
    public List<string> Tags { get; set; } = new List<string> { };

    // These are SECONDRY characteristics. Recomputed at every EFrame.

    // Is this resource relevent. (used for UI)
    public RGrid RPool { get; protected set; }

    // RProducedLocal + RConsumedLocal = RDeltaLocal
    // RProducedTrade + RConsumedTrade = RDeltaTrade
    // RDeltaLocal + RDeltaTrade = RDeltaTotal

    // NET amount of resource in storage.
    public List<StorageElement> RStorage { get; protected set; } = new();

    // Used to carry through numbers to next step.
    protected Dictionary<int, double> productionBuffer = new();
    protected Dictionary<int, double> consumptionBuffer = new();
    // Helper methods
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;

    public class RVec<TType> where TType : Resource.IResource
    {
        public Resource.RGroupList<TType> local = new();
        public Resource.RGroupList<TType> trade = new();
        public Resource.RGroupList<TType> total = new();
        public void Clear()
        {
            local.Clear();
            trade.Clear();
            total.Clear();
        }
        public void AddType(int type)
        {
            local[type] = new Resource.RGroup<TType>(type);
            trade[type] = new Resource.RGroup<TType>(type);
            total[type] = new Resource.RGroup<TType>(type);
        }
        public void Insert(TType r)
        {
            local.Insert(r);
            trade.Insert(r);
            total.Insert(r);
        }
    }

    public class RGrid
    {
        public List<int> resourcePresent = new();
        public RVec<Resource.IResource> produced = new();
        public RVec<Resource.IRequestable> consumed = new();
        public RVec<Resource.IResource> delta = new();
        public void AddType(int type)
        {
            resourcePresent.Add(type);
            produced.AddType(type);
            consumed.AddType(type);
            delta.AddType(type);
        }
    }
    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();

        RPool = new RGrid();

        Name = $"{Body.Name} station";

        if (ValidTradeReceiver)
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
        // Initial storage count.
        // GetStorage();
        // foreach (KeyValuePair<int, double> kvp in StartingResources)
        // {
        //     RStorage[kvp.Key].Deposit(kvp.Value);
        // }
    }
    public void RegisterTransformer(Resource.IResourceTransformers c)
    {
        Transformers.Add(c);
    }
    public void DeregisterTransformer(Resource.IResourceTransformers c)
    {
        Transformers.Remove(c);
    }
    public void RegisterIndustry(Industry i)
    {
        AddChild(i);
        Industries.Add(i);
        RegisterTransformer(i);
    }
    public void DeregisterIndustry(Industry i)
    {
        RemoveChild(i);
        Industries.Remove(i);
        DeregisterTransformer(i);
    }
    public void RegisterUpline(TradeRoute i)
    {
        UplineTraderoute = i;
        RegisterTransformer(i.TransformerTail);
    }
    public void DeregisterUpline(TradeRoute i, bool upline = false)
    {
        UplineTraderoute = null;
        DeregisterTransformer(i.TransformerHead);
    }
    public void RegisterDownline(TradeRoute i)
    {
        DownlineTraderoutes.Add(i);
        RegisterTransformer(i.TransformerHead);
    }
    public void DeregisterDownline(TradeRoute i)
    {
        DownlineTraderoutes.Remove(i);
        DeregisterTransformer(i.TransformerHead);
    }

    // this is so messy, i hate it. aaaaaaaaaaah
    public override void EFrame()
    {


        productionBuffer.Clear();

        foreach (Resource.IResource r in RPool.produced.total)
        {
            productionBuffer[r.Type] = r.Sum;
        }

        RPool.delta.Clear();
        RPool.consumed.Clear();
        RPool.produced.Clear();

        // GetStorage();

        GetProducers();
        GetConsumers();
    }

    // void GetStorage()
    // {
    //     foreach (Industry industry in Industries)
    //     {
    //         if (industry.stored == null) { continue; }
    //         foreach (Resource.RStatic output in industry.stored)
    //         {
    //             resourceStorage[output.Type].Add(output);
    //         }
    //     }
    // }

    // A resource is not tracked until it is used.


    void GetProducers()
    {
        foreach (Resource.IResourceTransformers rp in Transformers)
        {
            foreach (Resource.IResource output in rp.Production)
            {
                RPool.produced.trade.Insert(output);
                RPool.produced.total.Insert(output);
                RPool.delta.trade.Insert(output);
                RPool.delta.total.Insert(output);
            }
        }
    }

    void GetConsumers()
    {
        // Running total, first step.
        consumptionBuffer = new();

        foreach (Resource.IResourceTransformers rp in Transformers)
        {
            foreach (Resource.IRequestable input in rp.Consumption)
            {
                RPool.consumed.trade.Insert(input);
                RPool.consumed.total.Insert(input);
                RPool.delta.trade.Insert(input);
                RPool.delta.total.Insert(input);
            }
        }

        foreach (Resource.RGroup<Resource.IResource> i in RPool.delta.total.Standard)
        {
            consumptionBuffer[i.Type] = i.Sum;
            // double deficit = productionBuffer[i] - consumptionBuffer[i];
        }

    }
    // foreach (KeyValuePair<int, double> kvp in resourceBuffer)
    // {
    //     resourceStorage[kvp.Key].Deposit(kvp.Value);
    // }

    // // Amount of extra DELTA required to cover this request.
    //                 double remainderDelta = requested - resourceBuffer[type]; //+ it.Request.Sum;

    //                 // Amount of extra NET required to cover this request.	
    //                 // No extra required
    //                 if (remainderDelta <= 0)
    //                 {
    //                     input.Respond();
    //                     resourceBuffer[type] -= requested;
    //                 }
    //                 // Covering shortfall out of storage.
    //                 else if (storage.Stock() >= remainderDelta)
    //                 {
    //                     input.Respond();
    //                     //storage.Deposit(remainderDelta);
    //                     resourceBuffer[type] -= requested;
    //                 }
    //                 // Partial cover shortfall out of storage.
    //                 else
    //                 {
    //                     // Fullfill partial request.
    //                     input.Respond(storage.Stock());
    //                     //storage.Deposit(-storage.Stock());
    //                     resourceBuffer[type] = 0;
    //                 }

    //                 RConsumedLocal[input.Type].Add(input);
    //                 RDelta[input.Type].Add(new Resource.RStaticInvert(input.Response));
    //                 // Deduct remainder from storage.
    //                 // Emit some sort of storage message.
    public class StorageElement
    {
        public double Capacity { get; protected set; }
        public double Level { get; protected set; }
        public StorageElement(double _capacity, double _level)
        {
            Level = _level;
            Capacity = _capacity;
        }
    }
}
