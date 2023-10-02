using Godot;
using System;

public partial class UIWindowTrade : UIWindow
{
	UIList<Installation> installationList;
	UIList<TradeRoute> tradeRouteList;

	static readonly PackedScene prefab_Insallation = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Listables/Installation/UIInstallationSmall.tscn");
	static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Listables/TradeRoute/UITradeRouteFull.tscn");

	Player player;

	public Button toggleButton;

	VBoxContainer tabReceivers;
	VBoxContainer tabTradeRoutes;

	public override void _Ready()
	{
		base._Ready();
		GetNode<Global>("/root/Global").Connect("EFrameEarly", new Callable(this, "EFrameEarly"));

		player = GetNode<Player>("/root/Global/Player");

		tabReceivers = GetNode<VBoxContainer>("TabContainer/Shipyards");
		tabTradeRoutes = GetNode<VBoxContainer>("TabContainer/Trade Routes");

		installationList = new();
		tradeRouteList = new();

		installationList.Vertical = true;
		tradeRouteList.Vertical = true;

		installationList.Init(player.tradeHeads, prefab_Insallation);
		tradeRouteList.Init(player.tradeRoutes, prefab_TradeRoute);

		tabReceivers.AddChild(installationList);
		tabTradeRoutes.AddChild(tradeRouteList);
	}

	protected override void OnCloseRequested()
	{
		base.OnCloseRequested();
		toggleButton.ButtonPressed = false;
	}

	public void EFrameEarly()
	{

	}

}
