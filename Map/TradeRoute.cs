using Godot;
using System;
using System.Collections.Generic;

public class TradeRoute : EcoNode
{
    [Export]
    public Installation destination;
    public Installation source;
    public TransformerTrade transformerSource;
    public TransformerTrade transformerDestintation;
    public List<Resource> Balance;
    public List<TransformerInputType.Base> consumptionSource;
    public List<TransformerInputType.Base> consumptionDestination;

    public Body Body { get { return GetParent<Body>(); } }
    public int Index { get { return GetIndex(); } }

    // Input class type.
    public class InputType : TransformerInputType.Base
    {
        public InputType(TransformerTrade _transformer, ResourceStatic _request) : base(_transformer, _request)
        {
            transformer = _transformer;
            Request = _request;
            Type = Request.Type;
        }
        public new void Respond(ResourceStatic _response, int _status)
        {
            ((TransformerTrade)transformer).twin.Production.GetType(Type).Sum = _response.Sum;
        }
    }

    // Tradeweight in KTonnes
    //public Resource importTradeWeight;
    //public Resource exportTradeWeight;
    public Resource tradeWeight;
    public float distance;
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

        MatchDemand();
        UpdateFreighterWeight();
    }

    public void DrawLine()
    {
        GetNode<Line2D>("Line2D").Points = new Vector2[] { destination.Position, source.Position };
    }
    public float GetFrieghterWeight()
    {
        float shipWeightImport = 0;
        float shipWeightExport = 0;
        foreach (Resource child in Balance)
        {
            if (child.Sum > 0)
            {
                shipWeightExport += child.Sum * Resources.ShipWeight(child.Type);
            }
            else
            {
                shipWeightImport += child.Sum * Resources.ShipWeight(child.Type);
            }
        }
        return -Math.Max(shipWeightExport, shipWeightImport);
    }

    public void Set()
    {
        foreach (Resource r in Balance)
        {
            if (r.Sum > 0)
            {
                consumptionSource.Add(new InputType(transformerSource, new ResourceStatic(r.Type, r.Sum, $"Trade to {destination.Name}")));
            }
            else
            {
                consumptionDestination.Add(new InputType(transformerSource, new ResourceStatic(r.Type, r.Sum, $"Trade to {destination.Name}")));

            }
        }
    }

    // IEnumerable<Resource> InvertBalance()
    // {
    //     foreach (Resource r in balance)
    //     {
    //         if (r.Type == 901) { continue; }//Ignore trade ship cost.
    //         yield return new ResourceStatic(r.Type, -r.Sum);
    //     }
    // }

    // void Sync(ResourceList lead, ResourceList follow)
    // {
    //     foreach (Resource r in balance)
    //     {
    //         if (r.Type == 901) { continue; }//Ignore trade ship cost.
    //         yield return new ResourceStatic(r.Type, -r.Sum);
    //     }
    // }

    // Sets trade route equal to destination deficit/production
    public void MatchDemand()
    {
        Balance.Clear();
        foreach (Resource r in destination.resourceDelta.GetStandard())
        {
            Balance.Add(new ResourceStatic(r.Type, r.Sum));
        }
    }

    void UpdateFreighterWeight()
    {
        tradeWeight = new ResourceStatic(901, GetFrieghterWeight());
        Balance.Add(tradeWeight);
    }
}
