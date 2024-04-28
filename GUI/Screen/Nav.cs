using Godot;
using System;
using System.Numerics;


public partial class Nav : VBoxContainer
{
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIAccordian.tscn");


	Map map;
	public override void _Ready()
	{
		map = GetNode<Map>("/root/Global/Map");
		foreach (Galaxy g in map.GetChildren())
		{
			GetNode<VBoxContainer>("VBoxContainer").AddChild(DrawGalaxy(g));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Draw()
	{

	}

	public UIAccordian DrawGalaxy(Galaxy g)
	{
		UIAccordian ui_g = prefab_UIAccordian.Instantiate<UIAccordian>();
		ui_g.GetNode<Button>("Button").Text = g.Name;
		ui_g.GetNode<Button>("Button").Flat = true;
		ui_g.GetNode<Button>("Button").ButtonPressed = true;
		ui_g.GetNode<Container>("Container").Visible = true;
		VBoxContainer vb = new VBoxContainer();

		foreach (PlanetarySystem ps in g.GetChildren())
		{
			vb.AddChild(DrawPlanetarySystem(ps));
		}
		ui_g.GetNode<Container>("Container").AddChild(vb);
		return ui_g;
	}
	public UIAccordian DrawPlanetarySystem(PlanetarySystem ps)
	{
		UIAccordian ui_ps = prefab_UIAccordian.Instantiate<UIAccordian>();
		ui_ps.GetNode<Button>("Button").Text = ps.Name;
		ui_ps.GetNode<Button>("Button").Flat = true;
		ui_ps.GetNode<Button>("Button").ButtonPressed = false;
		ui_ps.GetNode<Container>("Container").Visible = false;
		VBoxContainer vb = new VBoxContainer();
		foreach (SatelliteSystem ss in ps.GetChildren())
		{
			vb.AddChild(DrawSatelliteSystem(ss));
		}
		ui_ps.GetNode<Container>("Container").AddChild(vb);
		return ui_ps;
	}
	public Button DrawSatelliteSystem(SatelliteSystem ss)
	{
		UISatelliteSystemNav uiw = new UISatelliteSystemNav();
		uiw.satelliteSystem = (ss);
		uiw.canvasLayer = GetParent<CanvasLayer>();
		return uiw;
	}
}
