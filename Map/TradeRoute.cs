using Godot;
using System;
using System.Collections.Generic;

public partial class TradeRoute : EcoNode
{
    [Export]
    public Installation destination;
    public Installation source;
    public TransformerTrade transformerSource;
    public TransformerTrade transformerDestintation;
    public Resource.RStaticList<Resource.RStatic> Balance;
    public List<TransformerInputType.Base> consumptionSource;
    public List<TransformerInputType.Base> consumptionDestination;

    public Body Body { get { return GetParent<Body>(); } }
    public int Index { get { return GetIndex(); } }

    // Input class type.
    public partial class InputType : TransformerInputType.Base
    {
        public InputType(TransformerTrade _transformer, Resource.RStatic _request) : base(_transformer, _request)
        {

        }
        public new void Respond()
        {
            base.Respond();
            ((Resource.RStatic)((TransformerTrade)transformer).twin.Production[Type]).Set(Request.Sum());
        }
        public new void Respond(double value)
        {
            base.Respond(value);
            ((Resource.RStatic)((TransformerTrade)transformer).twin.Production[(Type)]).Set(Response.Sum());
        }
    }

    // Tradeweight in KTonnes
    //public Resource.IResource importTradeWeight;
    //public Resource.IResource exportTradeWeight;
    public Resource.RStatic tradeWeight;
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

        destination = _destination;
        source = _source;

        transformerDestintation = new TransformerTrade();
        transformerSource = new TransformerTrade();

        transformerSource.Init(this, true);
        transformerDestintation.Init(this);

        source.RegisterTransformer(transformerSource);
        destination.RegisterTransformer(transformerDestintation);
        destination.uplineTraderoute = this;

        distance = destination.GetParent<Body>().Position.DistanceTo(source.GetParent<Body>().Position);
        Name = $"Trade route from {transformerSource.Name} to {transformerDestintation.Name}";

        Balance = new Resource.RStaticList<Resource.RStatic>();
        MatchDemand();
        UpdateFreighterWeight();
    }

    public void DrawLine()
    {
        GetNode<Line2D>("Line2D").Points = new Vector2[] { destination.Position, source.Position };
    }
    public double GetFrieghterWeight()
    {
        double shipWeightImport = 0;
        double shipWeightExport = 0;
        foreach (Resource.IResource child in Balance)
        {
            if (child.Sum() > 0)
            {
                shipWeightExport += child.Sum() * Resource.ShipWeight(child.Type());
            }
            else
            {
                shipWeightImport += child.Sum() * Resource.ShipWeight(child.Type());
            }
        }
        return -Math.Max(shipWeightExport, shipWeightImport);
    }

    public void Set()
    {
        foreach (Resource.IResource r in Balance)
        {
            if (r.Sum() > 0)
            {
                consumptionSource.Add(new InputType(transformerSource, new Resource.RStatic(r.Type(), r.Sum(), $"Trade to {destination.Name}")));
            }
            else
            {
                consumptionDestination.Add(new InputType(transformerSource, new Resource.RStatic(r.Type(), r.Sum(), $"Trade to {destination.Name}")));

            }
        }
    }

    // IEnumerable<Resource> InvertBalance()
    // {
    //     foreach (Resource r in balance)
    //     {
    //         if (r .Type() == 901) { continue; }//Ignore trade ship cost.
    //         yield return new Resource.IResource.RStatic(r .Type(), -r.Sum());
    //     }
    // }

    // void Sync(Resource.RList lead, Resource.RList follow)
    // {
    //     foreach (Resource r in balance)
    //     {
    //         if (r .Type() == 901) { continue; }//Ignore trade ship cost.
    //         yield return new Resource.IResource.RStatic(r .Type(), -r.Sum());
    //     }
    // }

    // Sets trade route equal to destination deficit/production
    public void MatchDemand()
    {
        Balance.Clear();

        foreach (Resource.IResource r in destination.resourceDelta)
        {
            Balance[(r.Type())].Set(r.Sum());
        }
        //Balance.RemoveZeros();
    }

    void UpdateFreighterWeight()
    {
        tradeWeight = new Resource.RStatic(901, GetFrieghterWeight());
        Balance.Add(tradeWeight);
    }
}
