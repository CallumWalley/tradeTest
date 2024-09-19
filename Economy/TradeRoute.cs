using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TradeRoute : Entity
{

    [Export]
    public Domain Head { get; set; }

    [Export]
    public Domain Tail { get; set; }

    public Resource.RDict<RStaticTail> ListTail { get; protected set; }
    public Resource.RDict<RStaticHead> ListHead { get; protected set; }
    Resource.RStatic shipDemand = new Resource.RStatic(811, 0);
    double InboundShipDemand
    {
        get
        {
            // Do null check as this is called before init for some reason
            return (ListHead == null) ? 0 : -ListHead.Where(x => x.Request > 0).Sum(x => x.Request);
        }
    }
    double OutboundShipDemand
    {
        get
        {
            return (ListHead == null) ? 0 : ListHead.Where(x => x.Request < 0).Sum(x => x.Request); ;
        }
    }
    public Resource.IResource ShipDemand
    {
        get
        {
            shipDemand.Request = Math.Round(Math.Max(InboundShipDemand, OutboundShipDemand) * distance * 0.005, 2);
            shipDemand.Name = string.Format("Ships required.");
            shipDemand.Details = string.Format("{0:N1} required for indbound cargo, {1:N1} for outbound.", InboundShipDemand, OutboundShipDemand);
            return shipDemand;
        }
    }
    public int Order
    {
        get
        {
            return Head.Order;
        }
    }
    public string Network
    {
        get
        {
            return Head.Network;
        }
    }
    // public List<TradeInputType> consumptionSource;
    // public List<TradeInputType> consumptionDestination;
    public int Index { get { return GetIndex(); } }

    // Tradeweight in KTonnes
    //public Resource.IResource importTradeWeight;
    //public Resource.IResource exportTradeWeight;
    public double distance;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // UpdateFreighterWeight();
        DrawLine();
        // GetNode<Global>("/root/Global").Connect("EFrameEarly", callable: new Callable(this, "EFrameEarly"));
    }

    // TODO: Move parts that are shared with PlayerTrade.ValidTradeHead there.
    public void Init()
    {
        GetNode<Line2D>("Line2D").Width = 1;


        // Downline must be resistered first else upline doesn't know it is a trade netwrowk
        Head.Trade.RegisterDownline(this);
        Tail.Trade.RegisterUpline(this);

        distance = 10; // Tail.GetParent<Body>().Position.DistanceTo(Head.GetParent<Body>().Position);
        Name = $"Trade route from {Head.Name} to {Tail.Name}";

        shipDemand.Name = Name;
    }
    public void Init(Domain _head, Domain _tail)
    {
        (Head, Tail) = (_head, _tail);
        Init();
    }
    public void DrawLine()
    {
        if (Tail != null && Head != null)
        {
            GetNode<Line2D>("Line2D").Points = new Vector2[] { Tail.Position, Head.Position };
        }
    }
    public void ChangeName(string newName)
    {
        Name = newName;
    }

    /// <summary>
    /// Place where trade route gets to run it's logic.
    /// </summary>
    public void SetRequest()
    {

        // If set to automatic
        foreach (KeyValuePair<int, Resource.Ledger.Entry> item in Tail.Ledger)
        {
            SetValue(item.Key, item.Value.Upline);
        }
    }
    public void SetValue(int key, double value)
    {
        // so messy.
        if (ListHead.ContainsKey(key))
        {
            ListHead[key].Request = value;
        }
        else
        {
            RStaticHead head = new RStaticHead(key, this);
            RStaticTail tail = new RStaticTail(key, this);
            tail.twin = head;
            head.twin = tail;
            head.Request = value;

            ListHead.Add(head);
            ListTail.Add(tail);
        }
    }

    // public void SetHeadExport(int key, double value)
    // {
    //     if (!ListHeadGain.ContainsKey(index))
    //     {
    //         RStaticTail tail = new RStaticTail(index, tradeRoute);
    //         RStaticHead head = new RStaticHead(index, tradeRoute);
    //         tail.twin = head;
    //         head.twin = tail;
    //         members.Add(index, tail);
    //         tradeRoute.ListRequestHead.Add(head);
    //     }
    //     return members[index];
    // }
    // public double Weight
    // {
    //     get
    //     {
    //         return members.Sum(x => x.Value.Request);
    //     }
    // }

    /// TRADE ROUTE UNIQUE CLASSES
    /// 

    // Same as regular list except makes sure to add corresponding element.
    // When adding to tail, create corresponding in head. 
    // public class RListRequestTail<T> : Resource.RList<RStaticTail>
    // {

    // }
    /// <summary>
    /// Same as regular request except details linked to trade.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // class RListRequestHead<T> : Resource.RList<RStaticHead>
    // {
    // 	TradeRoute tradeRoute;
    // 	public RListRequestHead(int _type, TradeRoute _tradeRoute) : base(_type, 0)
    // 	{
    // 		tradeRoute = _tradeRoute;
    // 	}

    // }
    public class RStaticHead : Resource.RStatic
    {
        public TradeRoute tradeRoute;
        public RStaticTail twin;

        public RStaticHead() { }
        public RStaticHead(int _type = 0, TradeRoute _tradeRoute = null) : base(_type, 0)
        {
            tradeRoute = _tradeRoute;
        }
        /// <summary>
        /// Drives state of twin.
        /// </summary>
        public override int State
        {
            get { return base.State; }
            set
            {
                base.State = value;
                twin.State = value;
            }
        }

        public override void Respond()
        {
            base.Respond();
        }
        public override void Respond(double value)
        {
            base.Respond(value);
        }
        /// <summary>
        /// Drives state of twin.
        /// </summary>
        /// <param name="value"></param>
        public override void Set(double value)
        {
            base.Set(Math.Round(value, 2));

            twin.Set(Math.Round(-value, 2));
        }
        public override string Name { get { return string.Format("{0} {1}", (Request > 0) ? "Import from" : "Export to", tradeRoute.Tail.Name); } }
        public override string Details { get { return string.Format("{0} {1} {2}", Resource.Name(Type), (Request > 0) ? "Import from" : "Export to", tradeRoute.Tail.Name); } }

    }
    /// <summary>
    /// Drives RequestHead
    /// </summary>
    public class RStaticTail : Resource.RStatic
    {
        TradeRoute tradeRoute;
        public RStaticHead twin;

        public RStaticTail() : base() { }

        public RStaticTail(int _type = 0, TradeRoute _tradeRoute = null) : base(_type, 0)
        {
            tradeRoute = _tradeRoute;
            // Touch leder if non existant.
            tradeRoute.Head.Ledger.InitType(_type);
        }

        /// <summary>
        /// Tail controls requeust of twin.
        /// </summary>
        public override double Request
        {
            get { return base.Request; }
            set
            {
                if (twin == null) { return; }// to avoid error when being set. Maybe better way.
                twin.Request = -value;
                base.Request = value;
            }
        }
        public override string Name { get { return string.Format("{0} {1}", (Request > 0) ? "Import from" : "Export to", tradeRoute.Tail.Name); } }
        public override string Details { get { return string.Format("{0} {1} {2}", Resource.Name(Type), (Request > 0) ? "Import from" : "Export to", tradeRoute.Head.Name); } }

    }
}
