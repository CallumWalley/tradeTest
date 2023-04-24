using Godot;
using System;
using System.Collections.Generic;

public class Installation : EcoNode
{

	[Export]
	public bool isValidTradeReceiver = false;

	[Export]
	public bool storageFull = false;

	//for debuggin
	public Godot.Collections.Array Children { get { return GetChildren(); } }

	public List<string> tags;

	// contains only POSITIVE deltas. evaluated during EFrameLate, and cleared in next EFrameEarly.
	public ResourceList resourceDeltaProduced = new ResourceList();
	// contains only NEGATIVE deltas. evaluated during EFrameEarly, and cleared in next EFrameLate.
	public ResourceList resourceDeltaConsumed = new ResourceList();
	// Contains sum of deltas, used by UI elements.
	public ResourceList resourceDelta = new ResourceList();

	// NET amount of resource in storage.
	public ResourceList resourceStockpile = new ResourceList(true);
	// TOTAL storage available.
	public ResourceList resourceStorage = new ResourceList();


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
		// If added in inspector, could already be resources in storage.
		if (storageFull)
		{
			foreach (Resource r in resourceStockpile)
			{
				Resource typeStockpile = resourceStockpile.GetType(r.Type, true);
				Resource typeStorage = resourceStorage.GetType(r.Type, true);

				typeStockpile.Sum = Math.Min(Math.Max(typeStockpile.Sum + r.Sum, 0), typeStorage.Sum);
			}
		}

	}
	public void RegisterTransformer(Transformer tr)
	{
		AddChild(tr);
		foreach (Resource p in tr.Production){
			resourceDeltaProduced.Add(p);
		}
		foreach (TransformerInputType.Base p in tr.Consumption){
			resourceDeltaConsumed.Add(p.Response);
		}
		foreach (Resource p in tr.Storage){
			resourceStorage.Add(p);
		}
	}
	public void DeregisterTransformer(Transformer tr)
	{
		RemoveChild(tr);
		foreach (Resource p in tr.Production){
			resourceDeltaProduced.Remove(p);
		}
		foreach (TransformerInputType.Base p in tr.Consumption){
			resourceDeltaConsumed.Remove(p.Response);
		}
		foreach (Resource p in tr.Storage){
			resourceStorage.Remove(p);
		}
	}
	public IEnumerable<TransformerTrade> GetTradeRoutes()
	{
		foreach (Transformer t in GetChildren())
			if (t is TransformerTrade)
			{
				yield return transformerTrade;
			}
	}


	// this is so messy, i hate it. aaaaaaaaaaah
	public override void EFrameLate()
	{
		foreach (Resource r in resourceDelta)
		{
			Resource typeStockpile = resourceStockpile.GetType(r.Type, true);
			Resource typeStorage = resourceStorage.GetType(r.Type, true);

			typeStockpile.Sum = Math.Min(Math.Max(typeStockpile.Sum + r.Sum, 0), typeStorage.Sum);
			// r.Clear();
		}
		// // Re count storage
		// foreach (ResourceAgr r in resourceStorage)
		// {
		// 	r.Clear();
		// }
		// //Get production from each transformer.
		// foreach (Resource r in resourceDeltaProduced)
		// {
		// 	r.Clear();
		// }
		// foreach (Transformer transformer in GetChildren())
		// {
		// 	foreach (Resource r in transformer.Production)
		// 	{
		// 		resourceDeltaProduced.Add(r);
		// 		resourceDelta.Add(r);
		// 	}
		// 	foreach (Resource r in transformer.Storage)
		// 	{
		// 		resourceStorage.Add(r);
		// 	}
		// }
	}
	public override void EFrameEarly()
	{
		// foreach (Resource r in resourceDeltaConsumed)
		// {
		// 	r.Clear();
		// }
		foreach (Transformer transformer in GetChildren())
		{
			foreach (TransformerInputType.Base it in transformer.Consumption)
			{
				int type = it.Type;
				// How much of this resource was produced last step.
				float produced = resourceDeltaProduced.GetType(type, true).Sum;
				float consumed = resourceDeltaConsumed.GetType(type, true).Sum;
				float stockpile = resourceStockpile.GetType(type, true).Sum;
				float storage = resourceStorage.GetType(type, true).Sum;

				// Amount of extra DELTA required to cover this request.
				float remainderDelta = (produced + consumed) + it.Request.Sum;
				float remainderNet = stockpile + remainderDelta;

				GD.Print(String.Format("Produced: {0:N1}\nConsumed: {1:N1}\nRequested: {2:N1}\nDelta: {3:N1}\nStored: {4:N1}\nRemainder: {5:N1}", produced, consumed, it.Request.Sum, remainderDelta,storage,remainderNet));
				// Amount of extra NET required to cover this request.	
				if (remainderDelta >= 0 || remainderNet >= 0)
				{
					it.Respond();
				}
				else
				{
					// Fullfill partial request.
					it.Respond(Mathf.Max(it.Request.Sum + remainderNet, 0));
				}

				// Add response to lists.
				resourceDeltaConsumed.Add(it.Response);
				resourceDelta.Add(it.Response);

				// Deduct remainder from storage.
				// Emit some sort of storage message.
			}
		}
	}
}
