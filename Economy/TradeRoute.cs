using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TradeRoute : Node
{


    [Export]
    public Installation Head { get; set; }

    [Export]
    public Installation Tail { get; set; }
    Resource.RList<RRequestTail> listTailLoss;
    Resource.RList<RRequestTail> listTailGain;
    Resource.RList<RRequestHead> listHeadLoss;
    Resource.RList<RRequestHead> listHeadGain;

    public Resource.RList<RRequestTail> ListTailLoss { get { return listTailLoss; } }
    public Resource.RList<RRequestTail> ListTailGain { get { return listTailGain; } }
    public Resource.RList<RRequestHead> ListHeadLoss { get { return listHeadLoss; } }
    public Resource.RList<RRequestHead> ListHeadGain { get { return listHeadGain; } }
    Resource.RRequest shipDemand = new Resource.RRequest(901, 0);
    double InboundShipDemand
    {
        get
        {
            // Do null check as this is called before init for some reason
            return (ListHeadGain == null) ? 0 : -ListHeadGain.Sum(x => x.Request);
        }
    }
    double OutboundShipDemand
    {
        get
        {
            return (ListHeadLoss == null) ? 0 : ListHeadLoss.Sum(x => x.Request);
        }
    }
    public Resource.IRequestable ShipDemand
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

        distance = Tail.GetParent<Body>().Position.DistanceTo(Head.GetParent<Body>().Position);
        Name = $"Trade route from {Head.Name} to {Tail.Name}";

        listHeadGain = new Resource.RList<RRequestHead>();
        listHeadLoss = new Resource.RList<RRequestHead>();

        listTailGain = new Resource.RList<RRequestTail>();
        listTailLoss = new Resource.RList<RRequestTail>();

        shipDemand.Name = Name;
    }
    public void Init(Installation _head, Installation _tail)
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

    public void SetValue(int key, double value)
    {
        // so messy.
        if (listHeadGain.ContainsKey(key))
        {
            if (value >= 0)
            {
                listHeadGain[key].Request = value;

            }
            // IF NEEDS TO CHANGE LIST.
            else
            {
                listHeadLoss[key] = listHeadGain[key];
                listHeadGain.Remove(listHeadGain[key]);

                listTailGain[key] = listTailLoss[key];
                listTailLoss.Remove(listTailLoss[key]);

                listHeadLoss[key].Request = value;
            }

        }
        else if (listHeadLoss.ContainsKey(key))
        {
            if (value >= 0)
            {
                listHeadGain[key] = listHeadLoss[key];
                listHeadLoss.Remove(listHeadLoss[key]);

                listTailLoss[key] = listTailGain[key];
                listTailGain.Remove(listTailGain[key]);

                listHeadGain[key].Request = value;
            }
            else
            {
                listHeadLoss[key].Request = value;
            }
        }
        else
        {
            RRequestHead head = new RRequestHead(key, this);
            RRequestTail tail = new RRequestTail(key, this);
            tail.twin = head;
            head.twin = tail;
            head.Request = value;

            if (value >= 0)
            {
                listHeadGain.Add(head);
                listTailLoss.Add(tail);
            }
            else
            {
                listHeadLoss.Add(head);
                listTailGain.Add(tail);
            }
        }
    }

    // public void SetHeadExport(int key, double value)
    // {
    //     if (!ListHeadGain.ContainsKey(index))
    //     {
    //         RRequestTail tail = new RRequestTail(index, tradeRoute);
    //         RRequestHead head = new RRequestHead(index, tradeRoute);
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
    // public class RListRequestTail<T> : Resource.RList<RRequestTail>
    // {

    // }
    /// <summary>
    /// Same as regular request except details linked to trade.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // class RListRequestHead<T> : Resource.RList<RRequestHead>
    // {
    // 	TradeRoute tradeRoute;
    // 	public RListRequestHead(int _type, TradeRoute _tradeRoute) : base(_type, 0)
    // 	{
    // 		tradeRoute = _tradeRoute;
    // 	}

    // }
    public class RRequestHead : Resource.RRequest
    {
        public TradeRoute tradeRoute;
        public RRequestTail twin;

        public RRequestHead(int _type, TradeRoute _tradeRoute) : base(_type, 0)
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
    public class RRequestTail : Resource.RRequest
    {
        TradeRoute tradeRoute;
        public RRequestHead twin;

        public RRequestTail(int _type, TradeRoute _tradeRoute) : base(_type, 0)
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
