using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TradeRoute : Node
{
	[Export]
	public Installation Tail { get; set; }

	[Export]
	public Installation Head { get; set; }

	public RListRequestTail<RRequestTail> ListRequestTail;
	public Resource.RList<RRequestHead> ListRequestHead;

	Resource.RRequest shipDemand = new Resource.RRequest(901, 0);
	double InboundShipDemand
	{
		get
		{
			// Do null check as this is called before init for some reason
			return  (ListRequestTail == null) ? 0 : ListRequestTail.Where(x => x.Request < 0).Select(x => x.Request).DefaultIfEmpty(0).Sum();
		}
	}
	double OutboundShipDemand
	{
		get
		{	
			return (ListRequestTail == null) ? 0 : -ListRequestTail.Where(x => x.Request > 0).Select(x => x.Request).DefaultIfEmpty(0).Sum();
		}
	}
	public Resource.IRequestable ShipDemand
	{
		get
		{
			shipDemand.Request = Math.Max(InboundShipDemand, OutboundShipDemand) * distance * 0.01;
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

		// Balance = new Resource.RStaticList<Resource.RStatic>();

		GetNode<Line2D>("Line2D").Width = 1;

		// TransformerTail = new TransformerTradeRouteTail();
		// TransformerHead = new TransformerTradeRouteHead(TransformerTail);

		//source.RegisterIndustry(IndustrySource);
		//destination.RegisterIndustry(IndustryDestintation);

		// Downline must be resistered first else upline doesn't know it is a trade netwrowk
		Head.Trade.RegisterDownline(this);
		Tail.Trade.RegisterUpline(this);

		distance = Tail.GetParent<Body>().Position.DistanceTo(Head.GetParent<Body>().Position);
		Name = $"Trade route from {Head.Name} to {Tail.Name}";

		ListRequestHead = new Resource.RList<RRequestHead>();
		ListRequestTail = new RListRequestTail<RRequestTail>(this);

		// RequestExportToParent = (Resource.RList<Resource.RRequest>)Tail.Ledger.RequestExportToParent;
		// RequestImportFromParent = (Resource.RList<Resource.RRequest>)Tail.Ledger.RequestImportFromParent;

		// RequestImportFromChildren = (Resource.RList<Resource.RRequest>)Head.Ledger.RequestExportToParent;
		// RequestExportToChildren = (Resource.RList<Resource.RRequest>)Head.Ledger.RequestImportFromParent;
		//string.Format("{{0}} request", Head.Name), string.Format("{{0}} requested from {0}", Head.Name)
		//string.Format("{{0}} import", Tail.Name), string.Format("{{0}} imported from {0}", Tail.Name)
		// ResourceParent = new Resource.RList<Resource.RStatic>();
		// RequestParent = new Resource.RList<Resource.RRequest>();
		// ResourceParent.CreateMissing = true;
		// RequestParent.CreateMissing = true;

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

    /// TRADE ROUTE UNIQUE CLASSES
    /// 

    // Same as regular list except makes sure to add corresponding element.
    // When adding to tail, create corresponding in head. 
    public class RListRequestTail<T> : Resource.RList<RRequestTail>
    {
        TradeRoute tradeRoute;
        public RListRequestTail(TradeRoute _tradeRoute) : base()
        {
            tradeRoute = _tradeRoute;
        }
        protected override RRequestTail _Get(int index)
        {
            if (!members.ContainsKey(index))
            {
                RRequestTail tail = new RRequestTail(index, tradeRoute);
                RRequestHead head = new RRequestHead(index, tradeRoute);
                tail.twin = head;
                head.twin = tail;
                members.Add(index, tail);
                tradeRoute.ListRequestHead.Add(head);
            }
            return members[index];
        }
        public double Weight
        {
            get{
                return members.Sum( x => x.Value.Request );
            }
        }
    }
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

        public RRequestHead(int _type, TradeRoute _tradeRoute) : base( _type, 0 )
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
            base.Set(Math.Round(value,2));
            
            twin.Set(Math.Round(-value,2));
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
