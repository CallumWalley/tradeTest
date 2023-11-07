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


	public Resource.TradeRouteResourceList HeadExportRequest; // What tail is requesting from parent.
	public Resource.TradeRouteResourceList HeadImport;  // What head is importing from child;

	//public Resource.TradeRouteResourceList HeadRequest; // 

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
	public Resource.RStatic TradeWeight { get; set; } = new Resource.RStatic(901, 0);
	public double distance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdateFreighterWeight();
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

		HeadExportRequest = new Resource.TradeRouteResourceList(string.Format("{{0}} request", Head.Name), string.Format("{{0}} requested from {0}", Head.Name));
		HeadImport = new Resource.TradeRouteResourceList(string.Format("{{0}} import", Tail.Name), string.Format("{{0}} imported from {0}", Tail.Name));

		TradeWeight.Name = Name;
	}
	public void Init(Installation _head, Installation _tail)
	{
		(Head, Tail) = (_head, _tail);
		Init();
	}
	public void DrawLine()
	{
		GetNode<Line2D>("Line2D").Points = new Vector2[] { Tail.Position, Head.Position };
	}
	public void ChangeName(string newName)
	{
		Name = newName;
	}
	public void Set()
	{
		// foreach (Resource.IResource r in Balance)
		// {
		//     if (r.Sum > 0)
		//     {
		//         consumptionSource.Add(new TradeInputType(IndustrySource, new Resource.RStatic(r.Type, r.Sum, $"Trade to {destination.Name}")));
		//     }
		//     else
		//     {
		//         consumptionDestination.Add(new TradeInputType(IndustrySource, new Resource.RStatic(r.Type, r.Sum, $"Trade to {destination.Name}")));
		//     }
		// }
	}

	// IEnumerable<Resource> InvertBalance()
	// {
	//     foreach (Resource r in balance)
	//     {
	//         if (r .Type == 901) { continue; }//Ignore trade ship cost.
	//         yield return new Resource.IResource.RStatic(r .Type, -r.Sum);
	//     }
	// }

	// void Sync(Resource.RList lead, Resource.RList follow)
	// {
	//     foreach (Resource r in balance)
	//     {
	//         if (r .Type == 901) { continue; }//Ignore trade ship cost.
	//         yield return new Resource.IResource.RStatic(r .Type, -r.Sum);
	//     }
	// }

	// Sets trade route equal to destination deficit/production
	// public void MatchDemand()
	// {
	//     Balance.Clear();

	//     foreach (Resource.IResource r in Tail.RDelta)
	//     {
	//         Balance[r.Type].Set(r.Sum);
	//         //Balance[r.Type].Name = $"Trade to {tradeRoute.destination}"
	//     }
	//     //Balance.RemoveZeros();
	// }

	void UpdateFreighterWeight()
	{
		TradeWeight.Set(newValue: TradeRoutes.GetFrieghterWeight());
	}

	// public class TransformerTradeRoute : Resource.IResourceTransformers
	// {
	//     public TradeRoute tr;
	//     public TransformerTradeRoute()
	//     {
	//         tr = tradeRoute;
	//     }
	//     public IEnumerable<Resource.IRequestable> _Consumption(TransformerTradeRoute tr)
	//     {
	//         foreach (Resource.RGroup<Resource.IResource> r in tradeRoute.Tail.produced)
	//         {
	//             if (r.Sum > 0) { continue; }
	//             yield return new Resource.RRequestBase(new Resource.RStatic(r.Type, -r.Sum, tradeRoute.Name, tradeRoute.Name));
	//         }
	//     }
	//     public IEnumerable<Resource.IRequestable> _Production(TransformerTradeRoute tr)
	//     {
	//         foreach (Resource.RGroup<Resource.IResource> r in tradeRoute.Tail.consumed)
	//         {
	//             if (r.Sum > 0) { continue; }
	//             yield return new Resource.RRequestBase(new Resource.RStatic(r.Type, -r.Sum, tradeRoute.Name, tradeRoute.Name));
	//         }
	//     }

	//     public virtual Resource.RList<Resource.IResource> Production { get; set; }
	//     public virtual Resource.RList<Resource.IRequestable> Consumption { get; set; }
	//     public System.Object Driver { get { return tradeRoute; } }
	// }
	// public class TransformerTradeRouteHead : TransformerTradeRoute
	// {
	//     TransformerTradeRoute twin;

	//     public TransformerTradeRouteHead(TransformerTradeRoute _twin) : base()
	//     {
	//         twin = _twin;
	//     }
	//     public override Resource.RList<Resource.IRequestable> Consumption
	//     {
	//         get
	//         {
	//             return new Resource.RList<Resource.IRequestable>(_Consumption(twin));
	//         }
	//     }
	//     public override Resource.RList<Resource.IResource> Production
	//     {
	//         get
	//         {
	//             return new Resource.RList<Resource.IResource>(_Production(twin));
	//         }
	//     }
	// }
	// public class TransformerTradeRouteTail : TransformerTradeRoute
	// {
	//     public override Resource.RList<Resource.IRequestable> Consumption { get { return new Resource.RList<Resource.IRequestable>(_Consumption(this)); } }
	//     public override Resource.RList<Resource.IResource> Production { get { return new Resource.RList<Resource.IResource>(_Production(this)); } }

	// }

	// void BalanceUpdated()
	// {
	//     consumed.Clear();
	//     produced.Clear();

	//     if (isUpline)
	//     {
	//         foreach (Resource.RStatic r in tradeRoute.Balance)
	//         {
	//             if (r.Sum < 0 && isUpline)
	//             {
	//                 consumed[r.Type] = new TradeInputType(twin, r);
	//             }
	//             else if (r.Sum > 0 && !isUpline)
	//             {
	//                 produced[r.Type] = new Resource.RStatic(r.Type, r.Sum, $"Trade from {tradeRoute.Head.Name}");
	//             }
	//         }
	//     }
	//     else
	//     {
	//         foreach (Resource.RStatic r in tradeRoute.Balance)
	//         {
	//             if (r.Sum < 0 && isUpline)
	//             {
	//                 consumed[r.Type] = new TradeInputType(twin, new Resource.RStatic(r.Type, r.Sum, $"Trade to {tradeRoute.Head.Name}"));
	//             }
	//             else if (r.Sum > 0 && !isUpline)
	//             {
	//                 produced[r.Type] = new Resource.RStatic(r.Type, r.Sum, $"Trade from {tradeRoute.Tail.Name}");
	//             }
	//         }
	//     }
}
