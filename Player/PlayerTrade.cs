using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;

public partial class PlayerTrade : Node
{
	// Parent to trade routes.

	static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

	public List<Domain> Heads { get; set; } = new List<Domain>();
	public IEnumerable<TradeRoute> Routes
	{
		get
		{
			foreach (TradeRoute tradeRoute in GetChildren())
			{
				yield return tradeRoute;
			}
		}
	}

	// Initialise trade routes added in editor.

	public override void _Ready()
	{
		GetNode<Global>("/root/Global").Connect("Setup", callable: new Callable(this, "Setup"));
	}

	void Setup()
	{
		foreach (TradeRoute t in GetChildren())
		{
			t.Init();
		}
	}
	public void RegisterTradeRoute(Domain head, Domain tail)
	{
		TradeRoute newTradeRoute = ps_TradeRoute.Instantiate<TradeRoute>();
		newTradeRoute.Init(head, tail);
		AddChild(newTradeRoute);
		GD.Print("Registered trade route.");
	}
	public void DeregisterTradeRoute(TradeRoute tr)
	{
		tr.Head.Trade.DeregisterDownline(tr);
		tr.Tail.Trade.DeregisterUpline(tr);

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
