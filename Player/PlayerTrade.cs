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

	static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

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
		RegisterCommands();

	}

	void Setup()
	{
		foreach (TradeRoute t in GetChildren())
		{
		}
	}
	public void RegisterTradeRoute(Domain head, Domain tail)
	{
		TradeRoute newTradeRoute = ps_TradeRoute.Instantiate<TradeRoute>();
		newTradeRoute.Head = head;
		newTradeRoute.Tail = tail;
		AddChild(newTradeRoute);
		GD.Print("Registered trade route.");
	}
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

	private void RegisterCommands()
	{
		RegisteredCommands.AddCommand(typeof(Trade));
		TradeList.player = this;
		Extensible.RegisterExtension("trade", typeof(TradeList));
	}
	[Command("trade", "Trade", "trade")]
	[Argument("subcommand", "string", "The name of the subcommand to run.")]
	[Description("Main command for trade")]
	public sealed partial class Trade : Extensible, ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			var extensions = GetCommandExtensions("trade");

			if (extensions is null) return ICommand.Failure(string.Format("Subcommand [i]{0}[/i] not found.", data.Arguments["subcommand"]));

			if (extensions.TryGetValue((string)data.Arguments["subcommand"], out Type extension))
				return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

			return ICommand.Failure("Variable not found.");
		}
	}
	[Extension("list", "Lists trade routes", "Lists trade routes", new string[] { "ls" })]
	public sealed class TradeList : IExtension
	{
		public static PlayerTrade player;
		public CommandResult Execute(CommandData data)
		{
			bool atLeastOne = false;
			foreach (TradeRoute tr in player)
			{
				data.Terminal.Print(tr.ToString());
				atLeastOne = true;
			}
			if (!atLeastOne)
			{
				data.Terminal.Print("No Trade Routes");
			}
			return ICommand.Success();
		}
	}
	[Extension("remove", "Removes trade routes", "Removes trade routes", new string[] { "rm" })]
	[Argument("trade_route", "string", "The name of the trade route to remove.")]
	public sealed class TradeEnd : IExtension
	{
		public static PlayerTrade player;
		public CommandResult Execute(CommandData data)
		{
			TradeRoute tr = (TradeRoute)(player.GetNodeOrNull((NodePath)data.Arguments["trade_route"]));
			if (tr == null)
			{
				return ICommand.Failure(string.Format("No trade route named '{0}'.", data.Arguments["trade_route"]));
			}
			else
			{
				player.DeregisterTradeRoute(tr);
				return ICommand.Success();
			}

		}
	}
}
