using Godot;
using System;
using System.Collections.Generic;

public class Installation : EcoNode
{

    // public ResourceAgr freightersRequired;
    // public ResourceStatic freightersTotal;
    // public Installation installation;

    // public float freighterCapacity = 14;
    // static readonly PackedScene ps_tradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

    [Export]
    public bool isValidTradeReceiver = false;

    //for debuggin
    public Godot.Collections.Array Children { get { return GetChildren(); } }

    public List<string> tags;

    public List<ResourceAgr> resourceDeltaProduced = new List<ResourceAgr>();
    public List<ResourceAgr> resourceDeltaConsumed = new List<ResourceAgr>();
    public List<ResourceAgr> resourceDelta = new List<ResourceAgr>();

    // NET amount of resource in storage.
    public List<ResourceStatic> resourceStockpile = new List<ResourceStatic>();

    // TOTAL storage available.
    public List<ResourceAgr> resourceStorage = new List<ResourceAgr>();


    // Downline traderoutes
    public TransformerTrade transformerTrade;

    // Upline traderoute
    public TradeRoute uplineTraderoute;

    public Vector2 Position { get { return GetParent<Body>().Position; } }

    // BODY IS PARENT
    public Body Body { get { return GetParent<Body>(); } }

    public float shipWeight;

    // Convenience function. Makes children at start of scene into members.
    public override void _Ready()
    {
        base._Ready();
        if (isValidTradeReceiver)
        {
            GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").RegisterInstallation(this);
        }
    }
    public void RegisterTransformer(Transformer tr)
    {
        AddChild(tr);
    }
    public void DeregisterTransformer(Transformer tr)
    {
        RemoveChild(tr);
    }
    public IEnumerable<TransformerTrade> GetTradeRoutes()
    {
        foreach (Transformer t in GetChildren())
            if (t is TransformerTrade)
            {
                yield return transformerTrade;
            }
    }
    public ResourceAgr GetType(int code, List<ResourceAgr> list)
    {
        // if list is 
        foreach (ResourceAgr r in list)
        {
            if (r.Type == code)
            {
                return r;
            }
        }
        ResourceAgr newResource = new ResourceAgr(code, new List<Resource> { });
        list.Add(newResource);
        return newResource;
    }
    public ResourceStatic GetType(int code, List<ResourceStatic> list)
    {
        // if list is 
        foreach (ResourceStatic r in list)
        {
            if (r.Type == code)
            {
                return r;
            }
        }
        ResourceStatic newResource = new ResourceStatic(code, 0);
        list.Add(newResource);
        return newResource;
    }
    // Wrapper functions.
    public ResourceAgr GetTypeStorage(int code)
    {
        return GetType(code, resourceStorage);
    }
    public ResourceStatic GetTypeStockpile(int code)
    {
        return GetType(code, resourceStockpile);
    }
    public ResourceAgr GetTypeDelta(int code)
    {
        return GetType(code, resourceDelta);
    }
    // public IEnumerable<ResourceAgr> GetIncome(){
    // 	foreach (ResourceAgr r in members){
    // 		if (r.Sum > 0){
    // 			yield return r;
    // 		}
    // 	} 
    // }
    // public IEnumerable<ResourceAgr> GetExpenses(){
    // 	foreach (ResourceAgr r in members){
    // 		if (r.Sum < 0){
    // 			yield return r;
    // 		}
    // 	} 
    // }
    // public IEnumerable<Transformer> GetTradeRoutes(){
    // 	foreach (Transformer t in GetChildren()){
    // 		if (t is TransformerTrade){
    // 			yield return t;
    // 		} 
    // 	}
    // }
    // Get resources with code between range.
    public IEnumerable<ResourceAgr> GetRange(int min, int max)
    {
        foreach (ResourceAgr r in resourceDeltaProduced)
        {
            if (min <= r.Type && r.Type <= max)
            {
                yield return r;
            }
        }
    }

    public IEnumerable<ResourceAgr> GetStandard()
    {
        return GetRange(1, 100);
    }


	// this is so messy, i hate it. aaaaaaaaaaah
    public override void EFrameLate()
    {	
        foreach (ResourceAgr r in resourceDelta)
        {	
			ResourceStatic typeStockpile = GetTypeStockpile(r.Type);
			ResourceAgr typeStorage = GetTypeStorage(r.Type);

			typeStockpile.Sum = Math.Min(Math.Max(typeStockpile.Sum + r.Sum, 0), typeStorage.Sum);
            r.Clear();
        }
        // Re count storage
        foreach (ResourceAgr r in resourceStorage)
        {
            r.Clear();
        }
        //Get production from each transformer.
        foreach (ResourceAgr r in resourceDeltaProduced)
        {
            r.Clear();
        }
        foreach (Transformer transformer in GetChildren())
        {
            foreach (Resource r in transformer.Produced())
            {
                AddResource(r, resourceDeltaProduced);
                AddResource(r, resourceDelta);
            }
            foreach (Resource r in transformer.storage)
            {
                AddResource(r, resourceStorage);
            }
        }
    }
    public override void EFrameEarly()
    {
        foreach (ResourceAgr r in resourceDeltaConsumed)
        {
            r.Clear();
        }
        foreach (Transformer transformer in GetChildren())
        {
            foreach (Transformer.Requester r in transformer.Requests())
            {
                // How much of this resource was produced last step.
                float producedType = GetType(r.Type, resourceDeltaProduced).Sum;
                float consumedType = GetType(r.Type, resourceDeltaConsumed).Sum;
                ResourceStatic stockpileType = GetType(r.Type, resourceStockpile);
                float storageType = GetType(r.Type, resourceStorage).Sum;

                // Amount of extra DELTA required to cover this request.
                float remainderDelta = (producedType + consumedType) + r.Sum;
                // Amount of extra NET required to cover this request.	
                float remainderNet = stockpileType.Sum + remainderDelta;
                ResourceStatic rr;
                int status;
                if (remainderDelta >= 0 || remainderNet >= 0)
                {
                    // Fullfill full request.
                    rr = new ResourceStatic(r.Type, r.Sum);
                    status = 0;
                }
                else
                {
                    // Fullfill partial request.
                    rr = new ResourceStatic(r.Type, r.Sum + remainderNet);
                    status = 1;
                }
                r.Respond(rr, status);
                AddResource(rr, resourceDeltaConsumed);
                AddResource(rr, resourceDelta);

                // Deduct remainder from storage.
                // Emit some sort of storage message.
            }
		}
    }

    // Will add resource as a new element.
    private void AddResource(Resource _resource, List<ResourceAgr> list)
    {
        //Check if aggrigator already exists.
        GetType(_resource.Type, list).add.Add(_resource);
    }
    // Will modify value of resource.

    private void AddResource(Resource _resource, List<ResourceStatic> list)
    {
        //Check if aggrigator already exists.
        GetType(_resource.Type, list).Sum += _resource.Sum;
    }
}
