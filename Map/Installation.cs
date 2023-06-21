using Godot;
using System;
using System.Collections.Generic;

public partial class Installation : EcoNode
{

    [Export]
    public bool ValidTradeReceiver { get; protected set; } = false;

    [Export]
    public Godot.Collections.Dictionary<int, double> StartingResources { get; set; } = new();

    //Transformers contains list trade + industry

    // These are PRIMARY characteristics. To add or remove requires call of function.
    public List<Resource.IResourceTransformers> Transformers { get; }
    public List<Industry> Industries { get; }
    public TradeRoute UplineTraderoute { get; protected set; }
    public List<TradeRoute> DownlineTraderoutes { get; protected set; } = new();
    public List<string> Tags { get; set; } = new List<string> { };

    // These are SECONDRY characteristics. Recomputed at every EFrame.

    // Is this resource relevent. (used for UI)
    List<int> resourcePresent;
    public Dictionary<string, Dictionary<string, Resource.RGroupList<Resource.IResource>>> RPool { get; protected set; } = new();

    // contains only POSITIVE deltas. evaluated during EFrameLate, and cleared in next EFrameEarly.
    public Resource.RGroupList<Resource.IResource> RProducedLocal { get; protected set; } = new();
    // contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
    public Resource.RGroupList<Resource.IRequestable> RConsumedLocal { get; protected set; } = new();
    public Resource.RGroupList<Resource.IResource> RProducedTrade { get; protected set; } = new();
    public Resource.RGroupList<Resource.IRequestable> RConsumedTrade { get; protected set; } = new();
    public Resource.RGroupList<Resource.IResource> RDeltaLocal { get; protected set; } = new();
    public Resource.RGroupList<Resource.IResource> RDeltaTrade { get; protected set; } = new();
    public Resource.RGroupList<Resource.IResource> RDeltaTotal { get; protected set; } = new();

    // RProducedLocal + RConsumedLocal = RDeltaLocal
    // RProducedTrade + RConsumedTrade = RDeltaTrade
    // RDeltaLocal + RDeltaTrade = RDeltaTotal

    // NET amount of resource in storage.
    public List<StorageElement> RStorage { get; protected set; } = new();

    // Used to carry through numbers to next step.
    protected Dictionary<int, double> productionBuffer = new();

    // Helper methods
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();

        RPool["local"] =
        new(){
        { "consumed", new Resource.RGroupList<Resource.IResource>() { } },
        { "produced", new Resource.RGroupList<Resource.IResource>() { } },
        { "delta", new Resource.RGroupList<Resource.IResource>() { } }
        };
    }
    Name = $"{Body.Name} station";

        if (ValidTradeReceiver)
        {
            GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").RegisterInstallation(this);
}
// If added in inspector. Regester Industrys.
foreach (Industry t in Children)
{
    // Remove self so not trigger warning when added.
    RemoveChild(t);
    RegisterIndustry(t);
}
// Initial storage count.
GetStorage();
foreach (KeyValuePair<int, double> kvp in StartingResources)
{
    RStorage[kvp.Key].Deposit(kvp.Value);
}
    }
    public void RegisterTransformer(Resource.IResourceTransformers c)
{
    Transformers.Add(c);
}
public void DeregisterTransformer(Resource.IResourceTransformers c)
{
    Transformers.Remove(c);
}
public void RegisterIndustry(Industry i)
{
    AddChild(i);
    Industries.Add(i);
    RegisterTransformer(i);
}
public void DeregisterIndustry(Industry i)
{
    RemoveChild(i);
    Industries.Remove(i);
    DeregisterTransformer(i);
}
public void RegisterUpline(TradeRoute i)
{
    UplineTraderoute = i;
    RegisterTransformer(i.IndustryDestintation);
}
public void DeregisterUpline(TradeRoute i, bool upline = false)
{
    UplineTraderoute = null;
    DeregisterTransformer(i.IndustryDestintation);
}
public void RegisterDownline(TradeRoute i)
{
    DownlineTraderoutes.Add(i);
    RegisterTransformer(i.IndustrySource);
}
public void DeregisterDownline(TradeRoute i)
{
    DownlineTraderoutes.Remove(i);
    DeregisterTransformer(i.IndustrySource);
}

// this is so messy, i hate it. aaaaaaaaaaah
public override void EFrame()
{

    // contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
    // Contains sum of deltas, used by UI elements.
    //resourceDelta = new();

    // NET amount of resource in storage.

    RDeltaTotal.Clear();
    RDeltaLocal.Clear();
    RDeltaTrade.Clear();

    RConsumedTrade.Clear();
    RConsumedLocal.Clear();

    productionBuffer.Clear();

    foreach (Resource.RBase r in RConsumedLocal)
    {
        resourceBuffer[r.Type] = r.Sum;
    }

    RProducedLocal.Clear();

    GetStorage();

    GetProducers();

    GetConsumers();
}

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

// A resource is not tracked until it is used.
void AddType(int type)
{
    resourcePresent.Add(type);
    RProducedLocal[type] = new Resource.RGroup<Resource.IResource>(type);
    RConsumedLocal[type] = new Resource.RGroup<Resource.IRequestable>(type);
    RDeltaLocal[type] = new Resource.RGroup<Resource.IResource>(type);
    RProducedTrade[type] = new Resource.RGroup<Resource.IResource>(type);
    RConsumedTrade[type] = new Resource.RGroup<Resource.IRequestable>(type);
    RDeltaTrade[type] = new Resource.RGroup<Resource.IResource>(type);
    RDeltaTotal[type] = new Resource.RGroup<Resource.IResource>(type);
}

void GetProducers()
{
    foreach (Resource.IResourceTransformers rp in Transformers)
    {
        foreach (Resource.IResource output in rp.Production)
        {
            if (!resourcePresent.Contains(output.Type))
            {
                AddType(output.Type);
            }
            RProducedLocal.Insert(output);
            RDeltaLocal.Insert(output);
            RDeltaTotal.Insert(output);
        }
    }
}

void GetConsumers()
{
    // Running total, first step.
    Dictionary<int, double> consumptionBuffer = new();

    foreach (Resource.IResourceTransformers rc in Transformers)
    {
        foreach (Resource.IRequestable input in rc.Consumption)
        {
            if (!resourcePresent.Contains(input.Type))
            {
                AddType(input.Type);
                consumptionBuffer[input.Type] = 0;
            }
            RConsumedLocal.Insert(input);
            RDeltaLocal.Insert(input);
            RDeltaTotal.Insert(r: input);
            consumptionBuffer[input.Type] -= input.SumRequest;
        }
    }

    foreach (int i in resourcePresent)
    {
        double deficit = productionBuffer[i] - consumptionBuffer[i];
    }


    // foreach (KeyValuePair<int, double> kvp in resourceBuffer)
    // {
    //     resourceStorage[kvp.Key].Deposit(kvp.Value);
    // }
}
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
    public double Capacity { get; protected set; }
    public double Level { get; protected set; }
    StorageElement(double _capacity, double _level)
    {
        Level = _level;
        Capacity = _capacity;
    }
}

}
