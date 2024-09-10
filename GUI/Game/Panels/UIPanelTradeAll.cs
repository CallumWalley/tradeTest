using Godot;
using System;
using System.Linq;


/// <summary>
/// A list showing all active trade routes.
/// </summary>
public partial class UIPanelTradeAll : UIPanel
{
	ScrollContainer tradeRouteScroll;

	UIList<TradeRoute> tradeRouteList;

	MarginContainer noTradeRoutes;

	static readonly PackedScene prefab_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/TradeRoute/UITradeRouteFull.tscn");

	Player player;

	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");


		tradeRouteScroll = GetNode<ScrollContainer>("ScrollContainer");
		noTradeRoutes = GetNode<MarginContainer>("NoTradeRoutes");
		tradeRouteList = new UIList<TradeRoute>();
		tradeRouteList.Vertical = true;
		tradeRouteList.Init(player.trade.Routes, prefab_TradeRoute);
		tradeRouteScroll.AddChild(tradeRouteList);
	}


	public override void _Draw()
	{
		base._Draw();
		if (player.trade.Routes.Count() > 0)
		{
			noTradeRoutes.Visible = false;
			tradeRouteScroll.Visible = true;
		}
		else
		{
			noTradeRoutes.Visible = true;
			tradeRouteScroll.Visible = false;
		}
	}
	public override void OnEFrameUpdate()
	{
		tradeRouteList.Update();
	}

}
