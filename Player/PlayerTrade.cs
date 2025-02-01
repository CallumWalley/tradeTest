using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;
using YAT.Classes;
using YAT.Scenes;
using YAT.Enums;
using YAT.Helpers;
namespace Game;

public partial class PlayerTrade : Node, IEnumerable<TradeRoute>
{
	// Parent to trade routes.
	public List<Domain> Heads { get; set; } = new List<Domain>();

	public IEnumerator<TradeRoute> GetEnumerator()
	{
		foreach (TradeRoute f in GetChildren())
		{
			yield return f;
		}
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	// Initialise trade routes added in editor.

	public override void _Ready()
	{
		GetNode<Global>("/root/Global").Connect("Setup", callable: new Callable(this, "Setup"));
	}

	[GameAttributes.Command]
	public TradeRoute RegisterTradeRoute(Domain head, Domain tail)
	{
		TradeRoute newTradeRoute = new TradeRoute();
		newTradeRoute.Head = head;
		newTradeRoute.Tail = tail;
		AddChild(newTradeRoute);
		GD.Print("Registered trade route.");
		return newTradeRoute;
	}
	[GameAttributes.Command]
	public void DeregisterTradeRoute(TradeRoute tr)
	{
		tr.Head.DeregisterDownline(tr);
		tr.Tail.DeregisterUpline(tr);

		RemoveChild(tr);
		tr.QueueFree();
		GD.Print("Removed trade route.");
	}

	public partial class ValidTradeHead
	{
		PlayerTrade player;
		public Domain Tail { get; private set; }
		public Domain Head { get; private set; }
		public Resource.RStatic TradeWeight { get; set; } = new Resource.RStatic(811, 0);

		//TODO: Might be tidier way to get parent instance?
		public ValidTradeHead(PlayerTrade _player, Domain _head, Domain _tail)
		{
			(player, Head, Tail) = (_player, _head, _tail);
			distance = 10;//Tail.GetParent<Body>().Position.DistanceTo(Head.GetParent<Body>().Position);
		}
		public float distance;
		public void Create()
		{
			player.RegisterTradeRoute(Head, Tail);
		}
	}

	public IEnumerable<ValidTradeHead> GetValidTradeHeads(Domain Tail)
	{
		foreach (Domain head in Heads)
		{
			if (Tail == head) { continue; }
			yield return new ValidTradeHead(this, head, Tail);
		}
	}

}
