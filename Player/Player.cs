using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node
{
	public PlayerTech tech;
	public PlayerTrade trade;
	public List<ResourcePool> ResourcePools { get; set; } = new List<ResourcePool>();

	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		trade = GetNode<PlayerTrade>("PlayerTrade");
	}
}
