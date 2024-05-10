using Godot;
using System;


/// <summary>
/// A list showing all active trade routes.
/// </summary>
public partial class UIPanelTradeAll : UIPanel
{
	UIList<ResourcePool> ResourcePoolList;
	UIList<TradeRoute> tradeRouteList;

	static readonly PackedScene prefab_Insallation = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables//ResourcePool/UIResourcePoolSmall.tscn");
	static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");

	Player player;

	public Button toggleButton;

	VBoxContainer tabReceivers;
	VBoxContainer tabTradeRoutes;

	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");

		//tabReceivers = GetNode<VBoxContainer>("TabContainer/Shipyards");
		//tabTradeRoutes = GetNode<VBoxContainer>("Trade Routes");

		//ResourcePoolList = new();
		tradeRouteList = new();

		//ResourcePoolList.Vertical = true;
		tradeRouteList.Vertical = true;

		//ResourcePoolList.Init(player.trade.Heads, prefab_Insallation);
		tradeRouteList.Init(player.trade.Routes, prefab_TradeRoute);

		//tabReceivers.AddChild(ResourcePoolList);
		//tabTradeRoutes.AddChild(tradeRouteList);
	}
}
