using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;

public partial class PlayerZones : Node
{
	// Parent to trade routes.

	// static readonly PackedScene ps_TradeRoute = (PackedScene)GD.Load<PackedScene>("res://Map/TradeRoute.tscn");

	public List<ResourcePool> Zones { get; set; }

	// Initialise trade routes added in editor.

	public override void _Ready()
	{
		GetNode<Global>("/root/Global").Connect("EFrameSetup", callable: new Callable(this, "EFrameSetup"));
	}

	public void RegisterZone(ResourcePool zone)
	{
		Zones.Add(zone);
	}
}
