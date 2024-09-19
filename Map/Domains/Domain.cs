using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Domain : Entity, IEnumerable<FeatureBase>
{
    [Export]
    public bool Active;

    public Vector2 Position
    {
        get { return new Vector2(0, 0); }
    }
    // Can receive trade. Should be determined 
    [Export]
    // Can receive trade. Should be determined by port.
    bool _validTradeReceiver = false;     // Can receive trade. Should 

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
    // public List<Resource.IResourceTransformers> Transformers { get; } = new();

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

    // public Resource.RDict<Resource.RGroup<Resource.IResource>> resources = new Resource.RDict<Resource.RGroup<Resource.IResource>>();
    // public Resource.RDict<Resource.RGroup<Resource.IResource>> ResourcesLocal { get { return resources[]} set; }
    public Resource.Ledger Ledger = new();
    public _Trade Trade = new _Trade();

    // // Used to carry through numbers to next step.
    // protected Dictionary<int, double> productionBuffer = new();
    // protected Dictionary<int, double> consumptionBuffer = new();
    // Helper methods
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    // public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;

    Player player;
    Global global;

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();

        global = GetNode<Global>("/root/Global");
        global.Connect("Setup", new Callable(this, "Setup"));
        global.Connect("EFrameEarly", new Callable(this, "EFrameEarly"));
        global.Connect("EFrameLate", new Callable(this, "EFrameLate"));

        player = GetNode<Player>("/root/Global/Player");

        ValidTradeReceiver = _validTradeReceiver;

        // Name = $"{Body.Name} station";

        Ledger.Domain = this;
        Trade.Domain = this;
        // Initial storage count.
        foreach (KeyValuePair<int, double> kvp in StartingResources)
        {
            // Dummy call to make sure resource exists.
            var _ = Ledger[kvp.Key];
            Ledger.Storage[kvp.Key].Set(kvp.Value);
        }
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

    public void Setup() { }

    public IEnumerator<FeatureBase> GetEnumerator()
    {
        foreach (FeatureBase f in GetChildren())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Class for orgasnisng
    public class _Trade
    {
        public Domain Domain;
        public Resource.RGroup<Resource.IResource> shipDemand = new Resource.RGroup<Resource.IResource>("Trade vessels in use.");
        // needs custom ui element
        public Resource.RGroup<Resource.IResource> ShipDemand
        {
            get
            {
                return Domain.Ledger[811].LocalNet;
            }
        }

        public _Trade() { }
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
            Domain.Order = Math.Min(DownlineTraderoutes.Count, 1);

            //DeregisterTransformer(i.TransformerHead);
        }
        public void RegisterDownline(TradeRoute i)
        {
            // If made head of trade network, set order to 1;
            DownlineTraderoutes.Add(i);
            Domain.Order = 1;
            if (Domain.Order < 2)
            {

                GD.Print(string.Format("{0} is the head of the new '{1}' network.", Domain.Name, Domain.Network));
            }

            GD.Print("Downline registered");
        }
        public void DeregisterDownline(TradeRoute i)
        {
            DownlineTraderoutes.Remove(i);

            // If no longer part of a trade network, set order to 0;
            if (Domain.Order == 1 && DownlineTraderoutes.Count < 1)
            {
                GD.Print(string.Format("{0} is not longer part of a network, so order is set to '0'", Domain.Name));
                Domain.Order = 0;
            }
        }
    }
    private void NameNetwork()
    //Gnerates name for this trade network.
    {
        this.Network = Name + " Market";
    }

    public override string ToString() { return Name; }

    public void AddFeature(FeatureBase feature)
    {
        AddChild(feature);
    }

}
