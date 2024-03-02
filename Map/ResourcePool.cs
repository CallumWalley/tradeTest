using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ResourcePool : Node, IEnumerable<Feature>
{
    [Export]
    public bool Active;

    public Vector2 Position
    {
        get { return new Vector2(0, 0); }
    }

    [Export]

    bool _validTradeReceiver = false;
    public bool ValidTradeReceiver

    {
        get { return _validTradeReceiver; }
        protected set
        {
            if (value)
            {
                player.trade.Heads.Add(this);
            }
            else
            {
                player.trade.Heads.Remove(this);
            }
            _validTradeReceiver = value;
        }
    }

    [Export]
    public Godot.Collections.Dictionary<int, double> StartingResources { get; set; } = new();

    // Transformers contains list trade + industry

    // These are PRIMARY characteristics. To add or remove requires call of function.
    public List<Resource.IResourceTransformers> Transformers { get; } = new();

    private int _order = 0;
    public int Order
    {
        get { return _order; }
        set
        {
            _order = value;
            if (value == 0)
            {
                this.Network = "No market";
                return;
            }
            else if (value == 1)
            {
                NameNetwork();
            }

            // If this has downline trade routes, they need to be updated.
            foreach (TradeRoute tradeRoute in Trade.DownlineTraderoutes)
            {
                tradeRoute.Tail.Order = value + 1;
                tradeRoute.Tail.Network = Network;
                GD.Print(string.Format("{0} has had its order set to '{1}' by '{2}'", tradeRoute.Tail.Name, Order, Name));

            }
        }
    }

    public string Network { get; set; }

    public List<string> Tags { get; set; } = new List<string> { };
    // These are SECONDRY characteristics. Recomputed at every EFrame.

    // Is this resource relevent. (used for UI)

    // RProducedLocal + RConsumedLocal = RDeltaLocal
    // RProducedTrade + RConsumedTrade = RDeltaTrade
    // RDeltaLocal + RDeltaTrade = RDeltaTotal

    // NET amount of resource in storage.
    public Store Storage { get; protected set; } = new();

    public Resource.Ledger Ledger = new();
    // Used to carry through numbers to next step.
    protected Dictionary<int, double> productionBuffer = new();
    protected Dictionary<int, double> consumptionBuffer = new();
    // Helper methods
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    // public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public _Trade Trade;
    public double shipWeight;

    Player player;
    Global global;

    // public List<int> resourcePresent = new();
    // public List<Resource.IResource> resourceBuffer = new();
    // public Dictionary<int, double[]> resourceBuffer2 = new();
    // public List<Resource.IResource> produced = new();
    // public List<Resource.IRequestable> consumed = new();
    // public List<Resource.IResource> export = new();
    // public List<Resource.IRequestable> import = new();

    // These are aggrigates
    // public List<Resource.IResource> delta = new();
    // public List<Resource.IResource> traded = new();


    // public void AddType(int type)
    // {
    //     resourcePresent.Add(type);
    //     delta.AddType(type);
    // }
    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();

        global = GetNode<Global>("/root/Global");
        global.Connect("EFrameSetup", new Callable(this, "EFrameSetup"));
        global.Connect("EFrameEarly", new Callable(this, "EFrameEarly"));
        global.Connect("EFrameLate", new Callable(this, "EFrameLate"));

        player = GetNode<Player>("/root/Global/Player");


        ValidTradeReceiver = _validTradeReceiver;

        // Name = $"{Body.Name} station";

        Ledger.ResourcePool = this;
        // Initial storage count.
        // GetStorage();
        foreach (KeyValuePair<int, double> kvp in StartingResources)
        {
            // Dummy call to make sure resource exists.
            var _ = Ledger[kvp.Key];
            Ledger.Storage[kvp.Key].Set(kvp.Value);
        }
        Trade = new _Trade(this);
    }



    // this is so messy, i hate it. aaaaaaaaaaah
    public void EFrameEarly()
    {
        Logistics.ExportToParent.EFrameEarly(this);
    }
    public void EFrameLate()
    {
        Logistics.ExportToParent.EFrameLate(this);
    }

    public void EFrameSetup() { }



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

    public IEnumerator<Feature> GetEnumerator()
    {
        foreach (Feature f in GetChildren())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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





    // Class for orgasnisng
    public class _Trade
    {
        ResourcePool ResourcePool;
        public Resource.RGroupRequests<Resource.IRequestable> shipDemand = new Resource.RGroupRequests<Resource.IRequestable>("Trade vessels in use.");
        // needs custom ui element
        public Resource.RGroupRequests<Resource.IRequestable> ShipDemand
        {
            get
            {
                return ResourcePool.Ledger[901].LocalLoss;
            }
        }

        public _Trade(ResourcePool _ResourcePool) { ResourcePool = _ResourcePool; }
        public TradeRoute UplineTraderoute = null;
        public List<TradeRoute> DownlineTraderoutes = new List<TradeRoute>();
        public void RegisterUpline(TradeRoute i)
        {
            UplineTraderoute = i;
            // Order is set in setter of parent order.
        }
        public void DeregisterUpline(TradeRoute i, bool upline = false)
        {
            UplineTraderoute = null;

            // 0 is not in network. 
            ResourcePool.Order = Math.Min(DownlineTraderoutes.Count, 1);

            //DeregisterTransformer(i.TransformerHead);
        }
        public void RegisterDownline(TradeRoute i)
        {
            // If made head of trade network, set order to 1;
            DownlineTraderoutes.Add(i);
            ResourcePool.Order = 1;
            if (ResourcePool.Order < 2)
            {

                GD.Print(string.Format("{0} is the head of the new '{1}' network.", ResourcePool.Name, ResourcePool.Network));
            }

            GD.Print("Downline registered");
        }
        public void DeregisterDownline(TradeRoute i)
        {
            DownlineTraderoutes.Remove(i);

            // If no longer part of a trade network, set order to 0;
            if (ResourcePool.Order == 1 && DownlineTraderoutes.Count < 1)
            {
                GD.Print(string.Format("{0} is not longer part of a network, so order is set to '0'", ResourcePool.Name));
                ResourcePool.Order = 0;
            }
        }
    }
    private void NameNetwork()
    //Gnerates name for this trade network.
    {
        this.Network = Name + " Market";
    }

    public override string ToString() { return Name; }
}
