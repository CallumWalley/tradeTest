using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class TradeRoute : EcoNode
{
    [Export]
    public Installation Tail { get; set; }
    public Installation Head { get; set; }
    public TradeRouteConsumer IndustrySource;
    public TradeRouteConsumer IndustryDestintation;
    public Resource.RStaticList<Resource.RStatic> Balance;
    // public List<TradeInputType> consumptionSource;
    // public List<TradeInputType> consumptionDestination;

    public Body Body { get { return GetParent<Body>(); } }
    public int Index { get { return GetIndex(); } }


    // Tradeweight in KTonnes
    //public Resource.IResource importTradeWeight;
    //public Resource.IResource exportTradeWeight;
    public Resource.RStatic TradeWeight { get; set; }
    public double distance;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        DrawLine();
    }
    // Called from TradeReciver
    public void Init(Installation _destination, Installation _source)
    {
        Tail = _destination;
        Head = _source;

        Balance = new Resource.RStaticList<Resource.RStatic>();


        GetNode<Line2D>("Line2D").Width = 1;

        IndustrySource = new TradeRouteConsumer(IndustryDestintation, this, false);
        IndustryDestintation = new TradeRouteConsumer(IndustrySource, this, true);

        //source.RegisterIndustry(IndustrySource);
        //destination.RegisterIndustry(IndustryDestintation);
        Tail.RegisterUpline(this);
        Head.RegisterDownline(this);

        distance = Tail.GetParent<Body>().Position.DistanceTo(Head.GetParent<Body>().Position);
        Name = $"Trade route from {IndustrySource} to {IndustryDestintation}";

        TradeWeight = new Resource.RStatic(901, GetFrieghterWeight(), $"{Name}");

        MatchDemand();
        UpdateFreighterWeight();

    }
    public void DrawLine()
    {
        GetNode<Line2D>("Line2D").Points = new Vector2[] { Tail.Position, Head.Position };
    }
    public double GetFrieghterWeight()
    {
        double shipWeightImport = 0;
        double shipWeightExport = 0;
        foreach (Resource.IResource child in Balance)
        {
            if (child.Sum > 0)
            {
                shipWeightExport += child.Sum;
            }
            else
            {
                shipWeightImport += child.Sum;
            }
        }
        return -Math.Max(shipWeightExport, shipWeightImport);
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
    public void MatchDemand()
    {
        Balance.Clear();

        foreach (Resource.IResource r in Tail.RDelta)
        {
            Balance[r.Type].Set(r.Sum);
            //Balance[r.Type].Name = $"Trade to {tradeRoute.destination}"
        }
        //Balance.RemoveZeros();
    }

    void UpdateFreighterWeight()
    {
        TradeWeight.Set(GetFrieghterWeight());
    }

    public class TradeRouteConsumer : Resource.IResourceTransformers
    {
        public TradeRouteConsumer twin;
        public TradeRoute tradeRoute;
        bool isUpline;

        Resource.RList<Resource.IResource> produced = new();
        Resource.RList<Resource.IRequestable> consumed = new();

        void BalanceUpdated()
        {
            consumed.Clear();
            produced.Clear();

            if (isUpline)
            {
                foreach (Resource.RStatic r in tradeRoute.Balance)
                {
                    if (r.Sum < 0 && isUpline)
                    {
                        consumed[r.Type] = new TradeInputType(twin, r);
                    }
                    else if (r.Sum > 0 && !isUpline)
                    {
                        produced[r.Type] = new Resource.RStatic(r.Type, r.Sum, $"Trade from {tradeRoute.Head.Name}");
                    }
                }
            }
            else
            {
                foreach (Resource.RStatic r in tradeRoute.Balance)
                {
                    if (r.Sum < 0 && isUpline)
                    {
                        consumed[r.Type] = new TradeInputType(twin, new Resource.RStatic(r.Type, r.Sum, $"Trade to {tradeRoute.Head.Name}"));
                    }
                    else if (r.Sum > 0 && !isUpline)
                    {
                        produced[r.Type] = new Resource.RStatic(r.Type, r.Sum, $"Trade from {tradeRoute.Tail.Name}");
                    }
                }
            }
        }

        public TradeRouteConsumer(TradeRouteConsumer _twin, TradeRoute _tradeRoute, bool _isUpline)
        {
            twin = _twin;
            tradeRoute = _tradeRoute;
            isUpline = _isUpline;
            BalanceUpdated();
        }
        public Resource.RList<Resource.IRequestable> Consumed()
        {
            return consumed;
        }
        public Resource.RList<Resource.IResource> Production
        {
            return produced;
        }
        public System.Object Driver()
        {
            return tradeRoute;
        }
    }
    public partial class TradeInputType : Resource.RRequestBase
    {
        TradeRouteConsumer tradeRouteConsumer;
        public TradeInputType(TradeRouteConsumer _tradeRouteConsumer, Resource.RStatic _request) : base(_request)
        {
            tradeRouteConsumer = _tradeRouteConsumer;
        }
        public new void Respond()
        {
            base.Respond();
            ((Resource.RStatic)tradeRouteConsumer.twin.Production[Type]).Set(Request.Sum);
        }
        public new void Respond(double value)
        {
            base.Respond(value);
            ((Resource.RStatic)tradeRouteConsumer.twin.Production[Type]).Set(value);
        }
    }
    // public class DownlineConsumer : TradeRouteConsumer
    // {
    //     public Resource.RList<Resource.IRequestable> Consumed() { }
    //     public Resource.RList<Resource.IResource> Production { }
    //     public System.Object Driver() { return this; }

    // }

    // public class TradeRouteConsumer : Resource.IResourceConsumer
    // {
    //     public TradeRoute tradeRoute;
    //     public TradeRouteConsumer twin;
    //     public bool isSource;
    //     string name;
    //     string description;
    //     List<TradeInputType> consumption;
    //     public List<Resource.IResourceConsumer> Consumers { get { return consumers; } }
    //     public System.Object Driver() { return this; }
    //     public void Init(TradeRoute _tradeRoute, bool _isSource = false)
    //     {
    //         tradeRoute = _tradeRoute;
    //         isSource = _isSource;
    //         if (isSource)
    //         {
    //             twin = tradeRoute.IndustryDestintation;
    //             consumption = tradeRoute.consumptionSource;
    //         }
    //         else
    //         {
    //             twin = tradeRoute.IndustrySource;
    //             consumption = tradeRoute.consumptionDestination;
    //         }
    //         description = $"A Trade Route connecting {tradeRoute.destination.Name}, {tradeRoute.destination.Body.Name} to {tradeRoute.source.Name}, {tradeRoute.source.Body.Name}";

    //         if (isSource)
    //         {
    //             name = $"Trade to {tradeRoute.destination.Body.Name}";
    //         }
    //         else
    //         {
    //             name = $"Trade from {tradeRoute.source.Body.Name}";
    //         }
    //     }

    //     public Resource.RList<Resource.BaseRequest> Consumed()
    //     {
    //         return consumption;
    //     }
    //     public Resource.RList<Resource.BaseRequest> Production
    //     {
    //         return twin.Consumed();
    //     }
    // }
}
