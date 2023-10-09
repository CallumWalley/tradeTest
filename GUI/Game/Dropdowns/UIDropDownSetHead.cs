using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UIDropDownSetHead : UIDropDown
{
	Player player;
	Installation installation;

	static readonly PackedScene prefab_UIValidTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UIValidTradeRoute.tscn");
	static readonly PackedScene prefab_UITradeRouteFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");

	public void Init(Installation _installation)
	{
		installation = _installation;
	}
	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");
		SetButtonContent();
	}
	public override void _AboutToPopup()
	{
		foreach (PlayerTrade.ValidTradeHead vth in player.trade.GetValidTradeHeads(installation))
		{
			UIValidTradeRoute vtr = prefab_UIValidTradeRoute.Instantiate<UIValidTradeRoute>();
			vtr.Init(vth);
			popupPanelList.AddChild(vtr);
			GD.Print($"Trade route from {vth.Head.Name} to {vth.Tail.Name}");
		}
		base._AboutToPopup();
	}

	public override void _PopupHide()
	{
		foreach (UIValidTradeRoute vth in popupPanelList.GetChildren())
		{
			vth.QueueFree();
		}
		base._PopupHide();
	}

	public override void _Draw()
	{
		base._Draw();
		if (installation.UplineTraderoute == null)
		{
			buttonDefault.Text = "No Upline Trade Route Set";
			buttonDefaultVisible = true;
		}
		else
		{
			buttonDefaultVisible = false;
		}

		if (player.trade.GetValidTradeHeads(installation).Count() < 1)
		{
			buttonDefault.Text = "No Upline Trade Routes Available";
			buttonDefaultVisible = true;
			buttonSettings.Disabled = true;
			buttonSettings.TooltipText = "No Valid Trade Hubs";
		}
		else
		{
			buttonSettings.Disabled = false;
			buttonSettings.TooltipText = "Set Upline Trade Hub";
		}

	}

	public void SetButtonContent()
	{
		foreach (Control child in buttonContent.GetChildren())
		{
			child.QueueFree();
		}
		if (installation.UplineTraderoute != null)
		{
			UITradeRouteFull uitrf = prefab_UITradeRouteFull.Instantiate<UITradeRouteFull>();
			uitrf.Init(installation.UplineTraderoute);
			buttonContent.AddChild(uitrf);
			// TODO make less messy.
			uitrf.cancelButton.Connect("pressed", callable: new Callable(this, "SetButtonContent"));
		}
		QueueRedraw();
	}
}
