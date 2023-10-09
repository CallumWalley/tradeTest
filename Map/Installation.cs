using Godot;
using System;
using System.Collections.Generic;

public partial class Installation : Node
{

    [Export]
    public bool ValidTradeReceiver { get; protected set; } = false;

    [Export]
    public Godot.Collections.Dictionary<int, double> StartingResources { get; set; } = new();

    //Transformers contains list trade + industry

    // These are PRIMARY characteristics. To add or remove requires call of function.
    public List<Resource.IResourceTransformers> Transformers { get; } = new();
    public List<Industry> Industries { get; } = new();
    public TradeRoute UplineTraderoute { get; set; } = null;
    public List<TradeRoute> DownlineTraderoutes { get; set; } = new();
    public List<string> Tags { get; set; } = new List<string> { };

    // These are SECONDRY characteristics. Recomputed at every EFrame.

    // Is this resource relevent. (used for UI)

    // RProducedLocal + RConsumedLocal = RDeltaLocal
    // RProducedTrade + RConsumedTrade = RDeltaTrade
    // RDeltaLocal + RDeltaTrade = RDeltaTotal

    // NET amount of resource in storage.
    public Store Storage { get; protected set; } = new();

    // Used to carry through numbers to next step.
    protected Dictionary<int, double> productionBuffer = new();
    protected Dictionary<int, double> consumptionBuffer = new();
    // Helper methods
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;


    public List<int> resourcePresent = new();
    public Dictionary<int, double> resourceBuffer = new();
    public List<Resource.IResource> produced = new();
    public List<Resource.IRequestable> consumed = new();
    public List<Resource.IResource> delta = new();

    // public void AddType(int type)
    // {
    //     resourcePresent.Add(type);
    //     delta.AddType(type);
    // }
    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();
        GetNode<Global>("/root/Global").Connect("EFrameEarly", new Callable(this, "EFrameEarly"));

        Name = $"{Body.Name} station";

        if (ValidTradeReceiver)
        {
            GetNode<Player>("/root/Global/Player").trade.Heads.Add(this);
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
        foreach (KeyValuePair<int, double> kvp in StartingResources)
        {
            Storage.Add(kvp.Key, kvp.Value);
        }
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
        //RegisterTransformer(i.TransformerTail);
    }
    public void DeregisterUpline(TradeRoute i, bool upline = false)
    {
        UplineTraderoute = null;
        //DeregisterTransformer(i.TransformerHead);
    }
    public void RegisterDownline(TradeRoute i)
    {
        DownlineTraderoutes.Add(i);
        GD.Print("Downline registered");
        //RegisterTransformer(i.TransformerHead);
    }
    public void DeregisterDownline(TradeRoute i)
    {
        DownlineTraderoutes.Remove(i);
        //DeregisterTransformer(i.TransformerHead);
    }

    // this is so messy, i hate it. aaaaaaaaaaah
    public void EFrameEarly()
    {
        // Add result of last step to storage.
        foreach (KeyValuePair<int, double> kvp in resourceBuffer)
        {
            Storage.Add(kvp.Key, kvp.Value);
            resourceBuffer.Remove(kvp.Key);
        }

        foreach (Resource.IResource r in produced)
        {
            productionBuffer[r.Type] = r.Sum;
        }

        delta.Clear();
        consumed.Clear();
        produced.Clear();

        GetProducers();
        GetConsumers();
        foreach (Resource.IResource r in delta)
        {
            if (r.Type < 500)
            {
                resourceBuffer.Add(r.Type, r.Sum);
            }
        }
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
                produced.Add(output);
                delta.Add(output);
            }
        }
    }

    void GetConsumers()
    {

        foreach (Resource.IResourceTransformers rp in Transformers)
        {
            foreach (Resource.IRequestable input in rp.Consumption)
            {
                consumed.Add(input);
                delta.Add(input);
            }
        }

        // foreach (Resource.RGroup<Resource.IResource> i in RPool.delta.total.Standard)
        // {
        //     consumptionBuffer[i.Type] = i.Sum;
        //     // double deficit = productionBuffer[i] - consumptionBuffer[i];
        // }

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
        public double Capacity { get; set; }
        public double Level { get; set; }

        //public int Type { get; protected set; }
        public StorageElement(double _capacity, double _level)
        {
            Level = _level;
            Capacity = _capacity;
        }
    }

    public class Store : IEnumerable<KeyValuePair<int, StorageElement>>
    {
        Dictionary<int, StorageElement> elements = new();

        public IEnumerator<KeyValuePair<int, StorageElement>> GetEnumerator()
        {
            foreach (KeyValuePair<int, StorageElement> element in elements)
            {
                yield return element;
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(int type, double sum)
        {
            if (elements.ContainsKey(type))
            {
                elements[type].Level += sum;
            }
            else
            {
                elements.Add(type, new StorageElement(1000, sum));
            }
        }
    }

}
