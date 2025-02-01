using Godot;
using System;
using System.Collections.Generic;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;
using YAT.Classes;
using YAT.Scenes;
using YAT.Enums;
using YAT.Helpers;
using System.Linq;
namespace Game;

public partial class Player : Node
{
	new public string Name { get { return base.Name; } set { base.Name = value; } }
	public PlayerTech tech;
	public PlayerTrade trade;
	public PlayerFeatureTemplates featureTemplates;

	public List<Domain> Domains { get; set; } = new List<Domain>();

	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		trade = GetNode<PlayerTrade>("PlayerTrade");
		featureTemplates = GetNode<PlayerFeatureTemplates>("PlayerFeatureTemplates");
		RegisterCommands();
	}

	public class Action
	{
		public IEnumerable<Parameter> Parameters { get; set; }

		public virtual bool Execute()
		{
			return false;
		}
	}

	public class Parameter
	{
		public IEnumerable<object> Options { get; }
		public virtual bool Valid()
		{
			return false;
		}
	}

	private void RegisterCommands()
	{
		TradeList.player = trade;
		TradeRemove.player = trade;
		TradeAdd.player = trade;

		Extensible.RegisterExtension("trade", typeof(TradeRemove));
		Extensible.RegisterExtension("trade", typeof(TradeList));
		Extensible.RegisterExtension("trade", typeof(TradeAdd));

		RegisteredCommands.AddCommand(typeof(Trade));

		BuildAdd.player = featureTemplates;

		Extensible.RegisterExtension("build", typeof(BuildAdd));
		Extensible.RegisterExtension("build", typeof(BuildResize));

		RegisteredCommands.AddCommand(typeof(Build));
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
				string name = tr.Name;
				player.DeregisterTradeRoute(tr);
				data.Terminal.Print($"Removed trade route {name}.");
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
				data.Terminal.Print($"Added trade route from {head.Name} to {tail.Name}");
				return ICommand.Success();
			}
		}
	}

	[Command("build", "Build")]
	[Argument("subcommand", "string", "The name of the subcommand to run.")]
	[Description("Main command for building templates")]
	public sealed partial class Build : Extensible, ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			var extensions = GetCommandExtensions("build");

			if (extensions.TryGetValue((string)data.Arguments["subcommand"], out Type extension))
				return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

			return ICommand.Failure(string.Format("Subcommand [i]{0}[/i] not found, valid options are {1}", data.Arguments["subcommand"], String.Join(",", extensions.Keys)));
		}
	}
	[Extension("add", "Add feature", "Adds a feature", new string[] { "a" })]
	public sealed class BuildAdd : IExtension
	{
		public static PlayerFeatureTemplates player;
		public CommandResult Execute(CommandData data)
		{
			if (data.RawData.Length < 3) { return ICommand.Failure("Not enough arguments"); }

			Node domain = World.SearchNode(data.Terminal.SelectedNode.Current, (string)data.RawData[1]);
			Node template = World.SearchNode(player, (string)data.RawData[2]);

			StringName name = new StringName("defaultName");

			if (data.RawData.Length > 3)
			{
				name = new StringName(data.RawData[3]);
			}


			if (domain == null)
			{
				return ICommand.Failure(string.Format("Could not find domain '{0}'.", data.RawData[1]));
			}
			else if (!(domain is Domain)) // && ((Domain)head).ValidTradeReceiver)
			{
				return ICommand.Failure(string.Format("'{0}' is not a valid domain", data.RawData[1]));
			}
			if (template == null)
			{
				return ICommand.Failure(string.Format("Could not find template '{0}'.\nValid options are {1}", data.RawData[2], String.Join(", ", player.GetChildren().Select(x => x.Name))));
			}
			else if (!player.GetValid((Entities.IPosition)domain).Contains((PlayerFeatureTemplate)template))
			{
				return ICommand.Failure(string.Format("'{0}' cannot be built at {1}, does not contain tag '{2}'.", data.RawData[2], data.RawData[1], String.Join("', '", ((PlayerFeatureTemplate)template).NeedsTags.Where(x => !((Entities.IPosition)domain).Tags.Contains(x)))));
			}
			else
			{
				ActionBuildNewIndustry abni = new ActionBuildNewIndustry();
				abni.Template = (PlayerFeatureTemplate)template;
				abni.OnAction();
				abni.Name = name;
				abni.Position = (Entities.IPosition)(domain);
				data.Terminal.Print($"Added new feature '{name}' to '{domain.Name}'.");
				return ICommand.Success();
			}
		}
	}
	[Extension("resize", "Changes the size of a feature. ", aliases: new string[] { "m" })]

	public sealed class BuildResize : IExtension
	{
		public CommandResult Execute(CommandData data)
		{
			double size = 1;
			if (data.RawData.Length < 1) { return ICommand.Failure("Not enough arguments"); }
			if (data.RawData.Length > 1)
			{
				if (!double.TryParse(data.RawData[2], out size))
				{
					return ICommand.InvalidArguments(string.Format("{0} not a number", data.RawData[2]));
				}
			}

			Node feature = World.SearchNode(data.Terminal.SelectedNode.Current, (string)data.RawData[1]);

			if (feature == null)
			{
				return ICommand.Failure(string.Format("Could not find feature '{0}'.", data.RawData[1]));
			}
			else if (feature is FeatureBase) // && ((Domain)head).ValidTradeReceiver)
			{
				return ICommand.Failure(string.Format("'{0}' is not a valid feature", data.RawData[1]));
			}
			else
			{
				ActionSetIndustrySize asis = new ActionSetIndustrySize();
				asis.Feature = (FeatureBase)feature;
				asis.NewScale = size;
				asis.OnAction();
				data.Terminal.Print($"'{feature}' '{size}'.");
				return ICommand.Success();
			}
		}
	}
}
