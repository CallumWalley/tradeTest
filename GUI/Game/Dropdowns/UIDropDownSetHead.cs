using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class UIDropDownSetHead : UIDropDown
{
	Player player;
	Domain Domain;

	static readonly PackedScene prefab_UIValidTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UIValidTradeRoute.tscn");
	static readonly PackedScene prefab_UITradeRouteFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");
	IEnumerable<PlayerTrade.ValidTradeHead> validTradeHeads;
	UITradeRouteFull uitrf;
	public void Init(Domain _Domain)
	{
		Domain = _Domain;
		SetButtonContent();
	}
	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");

	}
	public override void _AboutToPopup()
	{
		foreach (UIValidTradeRoute vth in popupPanelList.GetChildren())
		{
			vth.Free();
		}
		foreach (PlayerTrade.ValidTradeHead vth in validTradeHeads)
		{
			UIValidTradeRoute vtr = prefab_UIValidTradeRoute.Instantiate<UIValidTradeRoute>();
			vtr.Init(vth);
			vtr.Connect("pressed", Callable.From(() => OnButtonPressed(vth)));

			popupPanelList.AddChild(vtr);
		}
		base._AboutToPopup();
	}

	public override void _PopupHide()
	{
		base._PopupHide();
	}

	public override void _Draw()
	{
		base._Draw();
		validTradeHeads = player.trade.GetValidTradeHeads(Domain);
		if (Domain.UplineTraderoute == null)
		{
			buttonDefault.Text = "No Upline Trade Route Set";
			buttonDefaultVisible = true;
		}
		else
		{
			buttonDefaultVisible = false;
		}

		if (validTradeHeads.Count() < 1)
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

	public void OnButtonPressed(PlayerTrade.ValidTradeHead vth)
	{
		vth.Create();

		CloseRequested();
		SetButtonContent();
	}

	public void SetButtonContent()
	{
		foreach (Control child in buttonContent.GetChildren())
		{
			child.QueueFree();
		}
		if (Domain.UplineTraderoute != null)
		{
			uitrf = prefab_UITradeRouteFull.Instantiate<UITradeRouteFull>();
			uitrf.Init(Domain.UplineTraderoute);
			buttonContent.AddChild(uitrf);
			// TODO make less messy.
			uitrf.cancelButton.Connect("pressed", callable: new Callable(this, "SetButtonContent"));
		}
		Update();
	}

	public void Update()
	{
		if (uitrf != null) { uitrf.Update(); }
		QueueRedraw();
	}
}
