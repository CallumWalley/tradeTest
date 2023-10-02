using Godot;
using System;

public partial class Player : Node
{
	public PlayerTech tech;
	public PlayerTradeHeads tradeHeads;
	public PlayerTradeRoutes tradeRoutes;
	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		tradeHeads = GetNode<PlayerTradeHeads>("PlayerTradeHeads");
		tradeRoutes = GetNode<PlayerTradeRoutes>("PlayerTradeRoutes");
	}
}
