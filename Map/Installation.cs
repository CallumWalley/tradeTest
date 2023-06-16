using Godot;
using System;
using System.Collections.Generic;

public partial class Installation : EcoNode
{

    [Export]
    public bool isValidTradeReceiver = false;

    [Export]
    public bool storageFull = false;
    List<Industry> industries = new();

    //Transformers contains list trade + industry
    List<Resource.IResourceTransformers> transformers = new();

    public List<Resource.IResourceTransformers> Transformers { get { return transformers; } }

    public List<Industry> Industries { get { return industries; } }

    //for debuggin
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }

    public List<string> tags;

    // contains only POSITIVE deltas. evaluated during EFrameLate, and cleared in next EFrameEarly.
    public Resource.RGroupList<Resource.RGroup> resourceProduced = new();
    // contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
    public Resource.RGroupList<Resource.RGroup> resourceConsumed = new();
    // Contains sum of deltas, used by UI elements.
    public Resource.RGroupList<Resource.RGroup> resourceDelta = new();
    // NET amount of resource in storage.
    public Resource.RStorageList<Resource.RStorage> resourceStorage = new();

    Dictionary<int, double> resourceBuffer = new();

    // TOTAL storage available.

    // Downline traderoutes

    // Upline traderoute
    public TradeRoute UplineTraderoute;
    public List<TradeRoute> DownlineTraderoutes;


    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public double shipWeight;

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();
        resourceProduced.CreateMissing = true;
        resourceConsumed.CreateMissing = true;
        resourceDelta.CreateMissing = true;
        resourceStorage.CreateMissing = true;

        DownlineTraderoutes = new();

        Name = $"{Body.Name} station";

        if (isValidTradeReceiver)
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
        industries.Add(i);
        RegisterTransformer(i);
    }
    public void DeregisterIndustry(Industry i)
    {
        RemoveChild(i);
        industries.Remove(i);
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
    public override void EFrameLate()
    {

    }
    public override void EFrameEarly()
    {

        // contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
        // Contains sum of deltas, used by UI elements.
        //resourceDelta = new();

        // NET amount of resource in storage.


        resourceDelta.Clear();
        resourceConsumed.Clear();
        resourceStorage.Clear();

        resourceBuffer.Clear();

        foreach (Resource.RBase r in resourceProduced)
        {
            resourceBuffer[r.Type] = r.Sum;
        }

        resourceProduced.Clear();

        GetStorage();

        GetProducers();

        GetConsumers();

    }


    void GetStorage()
    {
        foreach (Industry industry in Industries)
        {
            if (industry.stored == null) { continue; }
            foreach (Resource.RStatic output in industry.stored)
            {
                resourceStorage[output.Type].Add(output);
            }
        }
    }

    void GetProducers()
    {
        foreach (Resource.IResourceTransformers rp in Transformers)
        {
            foreach (Resource.RBase output in rp.Produced())
            {
                resourceProduced[output.Type].Add(output);
                resourceDelta[output.Type].Add(output);
            }
        }
    }

    void GetConsumers()
    {
        foreach (Resource.IResourceTransformers rc in Transformers)
        {
            foreach (Resource.BaseRequest input in rc.Consumed())
            {
                int type = input.Request.Type;
                // How much of this resource was produced last step.
                Resource.RStorage storage = (Resource.RStorage)resourceStorage[type];
                double requested = input.Request.Sum;
                // this should just be somewhere else.
                if (!resourceBuffer.ContainsKey(type))
                {
                    resourceBuffer[type] = 0;
                }
                // Amount of extra DELTA required to cover this request.
                double remainderDelta = (resourceBuffer[type] + requested); //+ it.Request.Sum;

                // Amount of extra NET required to cover this request.	
                if (remainderDelta >= 0)
                {
                    input.Respond();
                    resourceBuffer[type] += requested;
                }
                else if (storage.Stock() >= -remainderDelta)
                {
                    input.Respond();
                    storage.Deposit(remainderDelta);
                    resourceBuffer[type] += requested;
                }
                else
                {
                    // Fullfill partial request.
                    input.Respond(-storage.Stock());
                    //storage.Deposit(-storage.Stock());
                    resourceBuffer[type] = 0;
                }

                resourceConsumed[input.Type].Add(input);
                resourceDelta[input.Type].Add(input);
                // Deduct remainder from storage.
                // Emit some sort of storage message.
            }
        }
        foreach (KeyValuePair<int, double> kvp in resourceBuffer)
        {
            resourceStorage[kvp.Key].Deposit(kvp.Value);
        }
    }
}
