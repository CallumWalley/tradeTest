using Godot;
using System;

public partial class Player : Node
{
	public PlayerTech tech;
	public PlayerTrade trade;
	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		trade = GetNode<PlayerTrade>("PlayerTrade");
	}
}
