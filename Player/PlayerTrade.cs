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
		RegisterCommands();

	}

	void Setup()
	{
		foreach (TradeRoute t in GetChildren())
		{
		}
	}
	[GameAttributes.Command]
	public void RegisterTradeRoute(Domain head, Domain tail)
	{
		TradeRoute newTradeRoute = new TradeRoute();
		newTradeRoute.Head = head;
		newTradeRoute.Tail = tail;
		AddChild(newTradeRoute);
		GD.Print("Registered trade route.");
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

	private void RegisterCommands()
	{
		TradeList.player = this;
		TradeRemove.player = this;
		TradeAdd.player = this;

		Extensible.RegisterExtension("trade", typeof(TradeRemove));
		Extensible.RegisterExtension("trade", typeof(TradeList));
		Extensible.RegisterExtension("trade", typeof(TradeAdd));

		RegisteredCommands.AddCommand(typeof(Trade));
	}
	[Command("trade", "Trade")]
	[Argument("subcommand", "string", "The name of the subcommand to run.")]
	[Description("Main command for trade")]
	public sealed partial class Trade : Extensible, ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			var extensions = GetCommandExtensions("trade");

			if (extensions.TryGetValue((string)data.Arguments["subcommand"], out Type extension))
				return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

			return ICommand.Failure(string.Format("Subcommand [i]{0}[/i] not found, valid options are {1}", data.Arguments["subcommand"], String.Join(",", extensions.Keys)));
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

	[Extension("remove", "Removes trade routes", "Removes trade route [i]trade_route[/i]", new string[] { "rm", "r" })]
	public sealed class TradeRemove : IExtension
	{
		public static PlayerTrade player;
		public CommandResult Execute(CommandData data)
		{
			if (data.RawData.Length < 2) { return ICommand.Failure("Not enough arguments"); }
			TradeRoute tr = (TradeRoute)(player.GetNodeOrNull((NodePath)data.RawData[1]));
			if (tr == null)
			{
				return ICommand.Failure(string.Format("No trade route named '{0}'.", data.RawData[1]));
			}
			else
			{
				player.DeregisterTradeRoute(tr);
				return ICommand.Success();
			}

		}
	}

	[Extension("add", "Add trade route", "Create a new trade route from [i]head_domain[/i] to [i]tail_domain[/i]", aliases: new string[] { "a" })]

	public sealed class TradeAdd : IExtension
	{
		public static PlayerTrade player;
		public CommandResult Execute(CommandData data)
		{
			if (data.RawData.Length < 3) { return ICommand.Failure("Not enough arguments"); }

			Node head = World.SearchNode(data.Terminal.SelectedNode.Current, (string)data.RawData[1]);
			Node tail = World.SearchNode(data.Terminal.SelectedNode.Current, (string)data.RawData[2]);

			if (head == null)
			{
				return ICommand.Failure(string.Format("Could not find '{0}'.", data.RawData[1]));
			}
			else if (!(head is Domain)) // && ((Domain)head).ValidTradeReceiver)
			{
				return ICommand.Failure(string.Format("'{0}' is not a valid trade receiver.", data.RawData[1]));
			}
			if (tail == null)
			{
				return ICommand.Failure(string.Format("Could not find '{0}'.", data.RawData[2]));
			}
			else if (!(tail is Domain)) // && ((Domain)tail).ValidTradeReceiver)
			{
				return ICommand.Failure(string.Format("'{0}' is not a valid trade receiver.", data.RawData[2]));
			}
			else
			{
				player.RegisterTradeRoute((Domain)head, (Domain)tail);
				return ICommand.Success();
			}
		}
	}
}
