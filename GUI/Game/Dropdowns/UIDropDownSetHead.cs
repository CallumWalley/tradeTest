using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UIDropDownSetHead : UIDropDown
{
	Player player;
	ResourcePool ResourcePool;

	static readonly PackedScene prefab_UIValidTradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UIValidTradeRoute.tscn");
	static readonly PackedScene prefab_UITradeRouteFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");
	IEnumerable<PlayerTrade.ValidTradeHead> validTradeHeads;

	public void Init(ResourcePool _ResourcePool)
	{
		ResourcePool = _ResourcePool;
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
		validTradeHeads = player.trade.GetValidTradeHeads(ResourcePool);
		if (ResourcePool.Trade.UplineTraderoute == null)
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

	public void SetButtonContent()
	{
		foreach (Control child in buttonContent.GetChildren())
		{
			child.QueueFree();
		}
		if (ResourcePool.Trade.UplineTraderoute != null)
		{
			UITradeRouteFull uitrf = prefab_UITradeRouteFull.Instantiate<UITradeRouteFull>();
			uitrf.Init(ResourcePool.Trade.UplineTraderoute);
			buttonContent.AddChild(uitrf);
			// TODO make less messy.
			uitrf.cancelButton.Connect("pressed", callable: new Callable(this, "SetButtonContent"));
		}
		QueueRedraw();
	}
}
