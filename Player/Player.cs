using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node
{
	public PlayerTech tech;
	public PlayerTrade trade;
	public List<Domain> Domains { get; set; } = new List<Domain>();

	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		trade = GetNode<PlayerTrade>("PlayerTrade");
	}
}
