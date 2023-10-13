using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node
{
	public PlayerTech tech;
	public PlayerTrade trade;
	public List<Installation> Installations { get; set; } = new List<Installation>();

	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		trade = GetNode<PlayerTrade>("PlayerTrade");
	}
}
