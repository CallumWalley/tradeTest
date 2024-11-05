using Godot;
using System;
namespace Game;

public partial class UIPanelDomainTrade : UIPanel
{
	public Domain domain;

	public UIDropDownSetHead uIDropDownSetHead;
	public UIList<TradeRoute> downlineRoutes;
	static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");

	public override void _Ready()
	{
		uIDropDownSetHead = GetNode<UIDropDownSetHead>("VBoxContainer/DropDown");
		uIDropDownSetHead.Init(domain);
		GetNode<UINetworkInfo>("VBoxContainer/NetworkInfo").domain = domain;
		downlineRoutes = new UIList<TradeRoute>();
		downlineRoutes.Vertical = true;
		downlineRoutes.Init(domain.DownlineTraderoutes, prefab_TradeRoute);
		GetNode<MarginContainer>("VBoxContainer/HBoxContainer/MarginContainer").AddChild(downlineRoutes);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void OnEFrameUI()
	{
		base.OnEFrameUI();
		downlineRoutes.Update();
		uIDropDownSetHead.Update();
	}
}
