using Godot;
using System;
using System.Numerics;


public partial class Nav : VBoxContainer
{
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/PlanetarySystem/UIAccordianPlanetarySystem.tscn");

	Map map;

	CanvasLayer canvasLayer;

	public override void _Ready()
	{
		map = GetNode<Map>("/root/Global/Map");
		canvasLayer = GetNode<CanvasLayer>("../");
		foreach (Galaxy g in map.GetChildren())
		{
			AddChild(DrawGalaxy(g));
		}
	}

	public HBoxContainer DrawGalaxy(Galaxy g)
	{
		HBoxContainer a = new HBoxContainer();
		VBoxContainer vb = new VBoxContainer();
		foreach (PlanetarySystem ps in g)
		{
			UIAccordianPlanetarySystem ui_ps = (UIAccordianPlanetarySystem)prefab_UIAccordian.Instantiate<UIAccordianPlanetarySystem>();
			ui_ps.planetarySystem = ps;
			ui_ps.canvasLayer = canvasLayer;
			vb.AddChild(ui_ps);
		}
		HBoxContainer hb = new HBoxContainer();
		hb.AddChild(new HSeparator());
		hb.AddChild(vb);
		a.AddChild(hb);
		return a;
	}
}
