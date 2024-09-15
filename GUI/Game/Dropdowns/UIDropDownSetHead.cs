using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UIDropDownSetHead : UIDropDown
{
	Player player;
	Domain Domain;

	static readonly PackedScene prefab_UIValidTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UIValidTradeRoute.tscn");
	static readonly PackedScene prefab_UITradeRouteFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");
	IEnumerable<PlayerTrade.ValidTradeHead> validTradeHeads;

	public void Init(Domain _Domain)
	{
		Domain = _Domain;
	}
	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");

		SetButtonContent();
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
			vtr.Driver = this;
			GD.Print(vtr.Name);
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
		if (Domain.Trade.UplineTraderoute == null)
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

	public void OnButtonPressed(Control listable)
	{
		((Lists.IListable<PlayerTrade.ValidTradeHead>)listable).GameElement.Create();

		CloseRequested();
		SetButtonContent();
	}

	public void SetButtonContent()
	{
		foreach (Control child in buttonContent.GetChildren())
		{
			child.QueueFree();
		}
		if (Domain != null && Domain.Trade.UplineTraderoute != null)
		{
			UITradeRouteFull uitrf = prefab_UITradeRouteFull.Instantiate<UITradeRouteFull>();
			uitrf.Init(Domain.Trade.UplineTraderoute);
			buttonContent.AddChild(uitrf);
			// TODO make less messy.
			uitrf.cancelButton.Connect("pressed", callable: new Callable(this, "SetButtonContent"));
		}
		QueueRedraw();
	}
}
