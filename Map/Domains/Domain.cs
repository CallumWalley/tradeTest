using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class Domain : Node2D, Entities.IEntityable, IEnumerable<Node>
{
    [ExportGroup("Economic")]
    [Export]
    public bool HasEconomy { get; set; }


    [Export]
    public string Description { get; set; }

    public virtual float CameraZoom { get { return 1; } }
    public virtual Godot.Vector2 CameraPosition { get { return GlobalPosition; } }
    CollisionShape2D collisionShape2D;
    // UIMapOverlayElement overlayElement;
    // public UIMapOverlayElement OverlayElement {
    //     get{
    //     if (overlayElement == null){
    //         overlayElement = new UIMapOverlayElement();
    //         overlayElement.element = this;
    //     }
    //     return overlayElement;
    //     }}

    // public override void _Process(double _delta)
    // {
    //     if (Input.IsActionPressed("ui_select"))
    //     {
    //         if (focus)
    //         {
    //             if (uiZone == null)
    //             {
    //                 uiZone = p_uiZone.Instantiate<UIWindowZone>();
    //                 uiZone.Init(this);
    //                 AddChild(uiZone);
    //             }
    //             uiZone.Visible = true;
    //             uiZone.MoveToForeground();
    //         }
    //     }
    //     //  && focus)
    //     // {
    //     // 	uiBody.Visible = true;
    //     // }
    // }

    // {
    //     get { return Spaceport; }
    //     protected set
    //     {
    //         if (value)
    //         {
    //             player.trade.Heads.Add(this);
    //         }
    //         else
    //         {
    //             player.trade.Heads.Remove(this);
    //         }
    //         Spaceport = value;
    //     }
    // }

    [Export]
    public Godot.Collections.Dictionary<int, double> StartingResources { get; set; } = new();

    // Transformers contains list trade + industry

    // These are PRIMARY characteristics. To add or remove requires call of function.
    // public List<Resource.IResourceTransformers> Transformers { get; } = new();

    private int _order = 0;

    /// <summary>
    /// 0 : no connections.
    /// 1 : head of network.
    /// </summary>
    public int Order
    {
        get { return _order; }
        set
        {
            _order = value;
            if (value == 0)
            {
                this.Network = null;
                return;
            }
            else if (value == 1)
            {
                Network = this;
                if (NetworkName == null)
                {
                    NetworkName = $"{Name} Trade Network";
                }
            }

            // If this has downline trade routes, they need to be updated.
            foreach (TradeRoute tradeRoute in DownlineTraderoutes)
            {
                tradeRoute.Tail.Order = value + 1;
                tradeRoute.Tail.Network = Network;
                GD.Print(string.Format("{0} has had its order set to '{1}' by '{2}'", tradeRoute.Tail.Name, Order, Name));
            }
        }
    }

    public Domain Network { get; set; }
    public string NetworkName { get; set; }


    public List<string> Tags { get; set; } = new List<string> { };
    // These are SECONDRY characteristics. Recomputed at every EFrame.

    // Is this resource relevent. (used for UI)

    // RProducedLocal + RConsumedLocal = RDeltaLocal
    // RProducedTrade + RConsumedTrade = RDeltaTrade
    // RDeltaLocal + RDeltaTrade = RDeltaTotal

    // public Resource.RDict<Resource.RGroup<Resource.IResource>> resources = new Resource.RDict<Resource.RGroup<Resource.IResource>>();
    // public Resource.RDict<Resource.RGroup<Resource.IResource>> ResourcesLocal { get { return resources[]} set; }
    public Resource.Ledger Ledger = new();

    // // Used to carry through numbers to next step.
    // protected Dictionary<int, double> productionBuffer = new();
    // protected Dictionary<int, double> consumptionBuffer = new();
    // Helper methods

    // public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;

    protected Player player;
    Global global;

    public override void _EnterTree()
    {
        base._EnterTree();

        Ledger.Domain = this;
    }

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();

        // Get nodes;
        global = GetNode<Global>("/root/Global");

        // Connect Signals
        global.Connect("Setup", new Callable(this, "Setup"));
        global.Connect("EFrameEarly", new Callable(this, "EFrameEarly"));
        global.Connect("EFrameLate", new Callable(this, "EFrameLate"));

        player = GetNode<Player>("/root/Global/Player");


        // Name = $"{Body.Name} station";
        // Initial storage count.
        foreach (KeyValuePair<int, double> kvp in StartingResources)
        {
            // Dummy call to make sure resource exists.
            // var _ = Ledger[kvp.Key];
            ((Resource.Ledger.EntryAccrul)Ledger[kvp.Key]).Stored.Sum = (kvp.Value + ((Resource.Ledger.EntryAccrul)Ledger[kvp.Key]).Stored.Sum);
        }
    }

    public override void _Process(double delta)
    {
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

    public IEnumerator<Node> GetEnumerator()
    {
        foreach (Node f in GetChildren())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Resource.RGroup<Resource.IResource> shipDemand = new Resource.RGroup<Resource.IResource>(802, "Trade vessels in use.");
    // needs custom ui element
    public Resource.RGroup<Resource.IResource> ShipDemand
    {
        get
        {
            return Ledger[811].LocalNet;
        }
    }

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
        Order = Math.Min(DownlineTraderoutes.Count, 1);

        //DeregisterTransformer(i.TransformerHead);
    }
    public void RegisterDownline(TradeRoute i)
    {
        // If made head of trade network, set order to 1;
        DownlineTraderoutes.Add(i);
        if (Order == 0) { Order = 1; GD.Print(string.Format("{0} is the head of the new '{1}' network.", Name, Network)); }

        GD.Print("Downline registered");
    }
    public void DeregisterDownline(TradeRoute i)
    {
        DownlineTraderoutes.Remove(i);

        // If no longer part of a trade network, set order to 0;
        if (Order == 1 && DownlineTraderoutes.Count < 1)
        {
            GD.Print(string.Format("{0} is not longer part of a network, so order is set to '0'", Name));
            Order = 0;
            Network = null;
        }
    }

    public FeatureBase this[int index]
    {
        get
        {
            return (FeatureBase)GetChild(index);
        }
    }
    public override string ToString() { return Name; }

    /// <summary>
    /// Creates a new feature from a template. Note, feature will be size 0. To give size, call ChangeSize()
    /// </summary>
    /// <param name="template"></param>
    /// <param name="name"></param>
    /// <param name="scale"></param>
    [GameAttributes.Command]
    public FeatureBase AddFeature(PlayerFeatureTemplate template, StringName name)
    {
        FeatureBase newFeature = template.Instantiate();
        newFeature.Template = template;
        newFeature.Name = name;

        // If has size. Set size to zero.
        if (newFeature.FactorsSingle.ContainsKey(901))
        {
            newFeature.FactorsSingle[901].Sum = 0;
        }
        AddChild(newFeature);
        return newFeature;
    }
}
